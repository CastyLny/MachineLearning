using System.Collections;
using System.Collections.Generic;
using UnityEngine;



//All the math functions
public class Maths
{

    public static float ReLu(float value)
    {
        return Mathf.Max(0f, value);
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

    public List<GameObject> foodsInRange = new List<GameObject>();

    public List<List<List<float>>> thetas = new List<List<List<float>>>();


    // Start is called before the first frame update
    void Start()
    {
        StartCoroutine("CountTimer");
        StartCoroutine("Increment");

        
    }

    // Update is called once per frame
    void Update()
    {
        move(Random.Range(-1f, 1f), Random.Range(-1f, 1f), Random.Range(10, 200) * Time.deltaTime);


        //Eat mechanism
        Collider[] foods = Physics.OverlapSphere(this.transform.position, 3f);
        foreach(Collider food in foods)
        {
            if(food.gameObject.tag == "Food")
            {
                Debug.Log("I ate");
                fullness += 10;
                Destroy(food.gameObject);
            }
        }

        Collider[] foodsSee = Physics.OverlapSphere(this.transform.position, 15f);
        foreach(Collider food in foodsSee)
        {
            if(food.gameObject.tag == "Food")
            {
                Debug.Log("I see food");
                foodsInRange.Add(food.gameObject);
            }
        }

    }


    //Handle death
    void Die()
    {
        Debug.Log("Die");

        GameObject parent = this.gameObject.transform.parent.gameObject;
        AiSpawner script = parent.GetComponent<AiSpawner>();
        script.HandleDeath(SurvivedTime);
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
                nodeProducts[node] = Maths.ReLu(Maths.PseudoVectorMult(thetas[i][node], input));//Basicly matrix multiplication, turning the both vector into scalar values
                
            }
            Debug.Log(nodeProducts);
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

    void move(float x, float z, float speed)
    {
        this.transform.position += new Vector3(x, 0, z) * speed;
    }


    



}


