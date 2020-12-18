using System.Collections;
using System.Collections.Generic;
using UnityEngine;



//All the math functions
public class Maths
{

    public static float Sigmoid(float X, float axis = 0, float amplitude = 1)
    {
        return (amplitude / 1+ Mathf.Exp(-X)) - axis;
    }

    public static float ReLu(float value)
    {
        return Mathf.Clamp(Mathf.Max(0f, value),0, 10f);
    }


    public static float PseudoVectorMult(List<float> first, List<float> second)
    {
        float product = 0f;

        for (int i = 0; i < first.Count; i++)
        {
            product += first[i] * second[i];
        }

        return product;
    }


}
public class AI : MonoBehaviour
{


    
    public float health = 100f;
    public float fullness = 100f;
    float fatness = 100f;
    public float SurvivedTime = 0f;
    public float fullnessLowerAmount = 1f;

    public GameObject targetedFood = null;

    public List<List<List<float>>> SpeedThetas = new List<List<List<float>>>();
    public List<List<List<float>>> XThetas = new List<List<List<float>>>();
    public List<List<List<float>>> ZThetas = new List<List<List<float>>>();
    public List<List<List<float>>> XRotThetas = new List<List<List<float>>>();




    // Start is called before the first frame update
    void Start()
    {
        StartCoroutine("CountTimer");
        StartCoroutine("Increment");

        
    }

    // Update is called once per frame
    void Update()
    {


        //Eat mechanism
        Collider[] foods = Physics.OverlapSphere(this.transform.position, 3f);
        foreach(Collider food in foods)
        {
            if(food.gameObject.tag == "Food")
            {
                
                fullness += 10;
                food foodScript = food.GetComponent<food>();
                foodScript.die();
            }else if(food.gameObject.tag == "Death")
            {
                Die();
            }
        }




        if(targetedFood == null)
        {
            Debug.Log("Checking for food");
            Collider[] foodsSee = Physics.OverlapSphere(this.transform.position, 60f);
            foreach (Collider food in foodsSee)
            {
                if (food.gameObject.tag == "Food")
                {
                    targetedFood = food.gameObject;


                }
            }
        }



        //Input the variables to the brain to generate the movement
        if(targetedFood != null)
        {
       
            List<float> input = new List<float> { 1f, targetedFood.transform.position.x - transform.position.x, targetedFood.transform.position.z - transform.position.z, health, fullness, fatness };
            List<float> XMovement = Brain(XThetas, input);
            List<float> ZMovement = Brain(ZThetas, input);
            List<float> SpeedMovement = Brain(SpeedThetas, input);
            List<float> XRotate = Brain(XRotThetas, input);
            //Debug.Log(XMovement[0].ToString() + ":" + ZMovement[0].ToString() + ":" + SpeedMovement[0].ToString() + ":" + XRotate[0].ToString());


            move(XMovement[0] * Time.deltaTime, ZMovement[0] * Time.deltaTime,Maths.ReLu(SpeedMovement[0]) * Time.deltaTime, XRotate[0] );
        }


    }


    //Handle death
    void Die()
    {
        Debug.Log("Die");

        GameObject parent = this.gameObject.transform.parent.gameObject;
        AiSpawner script = parent.GetComponent<AiSpawner>();
        script.HandleDeath(SurvivedTime,XThetas, ZThetas, SpeedThetas, XRotThetas);
        Destroy(this.gameObject);

    }

    //Neuronetwork that will determined the action of the mob
    List<float> Brain(List<List<List<float>>> thetas, List<float> input)
    {
        for(int i = 0; i < thetas.Count; i++)
        //for every layer/ loop per layer
        {
            List<float> nodeProducts = new List<float>(); 
            //in layer we got node which is just containing all the weight and bias in form of an array
            for(int node = 0; node < thetas[i].Count; node++)
            {
                //Basicly matrix multiplication, turning the both vector into scalar values
                
                nodeProducts.Add(Mathf.Clamp(Maths.PseudoVectorMult(thetas[i][node], input), -10f, 10f));
                
                
            }
            
            input = nodeProducts;
        }

        
        return input;
    }


    IEnumerator CountTimer()
    {
        while (true)
        {
            yield return new WaitForSeconds(0.01f);
            SurvivedTime += 0.01f;
        }
    }

    IEnumerator Increment()
    {
        while (true){
            //tick time on how the time update of health will be determined
            yield return new WaitForSeconds(0.5f);
            Debug.Log("Ticking");
            fullness -= fullnessLowerAmount;//How much fullness will be lower after determined amount of time

            //Program all the bare condition for living
            if (fullness <40f) {health -= 5f;}
            else if (fullness > 100f) {fatness += 5f;}
            else if (health < 100 && fullness > 80) {health += 5;}
            if (fatness > 160f) {health -= 5f;}
            if (health <= 0f) {Die();}

        }
    }

    void move(float x, float z, float speed, float xRot)
    {
        transform.position += Vector3.forward * x;
        transform.position += Vector3.right * z;
        Vector3 rotateBy = new Vector3(0, xRot, 0) * speed;
        transform.Rotate(rotateBy);
    }

   
  


    



}


