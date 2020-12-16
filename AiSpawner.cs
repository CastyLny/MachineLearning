using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AiSpawner : MonoBehaviour
{

    public GameObject mobPrefab;
    public int MobNum;
    public int generations;
    private bool StartNewGen = true;
    public List<float> surviveTimes = new List<float>();

    public int inputsDimension = 5;

    public List<int> layers = new List<int>();

    public List<List<List<float>>> bestThetas = new List<List<List<float>>>();


    // Start is called before the first frame update
    void Start()
    {
        //Generate random values
        bestThetas = GenerateRandomTheta(-3f, 3f);
    }

    // Update is called once per frame
    void Update()
    {
        if (StartNewGen)
        {
            StartNewGen = false;
            surviveTimes.Clear();
            for (int i = 0; i < MobNum; i++)
            {

                Vector3 position = new Vector3(0, 2, i * 2);
                GameObject mob = Instantiate(mobPrefab, position, Quaternion.identity);
                mob.transform.parent = this.transform;
                AI script = mob.GetComponent<AI>();

            }
        }

        else if (MobNum <= surviveTimes.Count)
        {
            StartNewGen = true;
        }
    }

    //function that are called from the child object when it dies
    public void HandleDeath(float survivedTime)
    {
        surviveTimes.Add(survivedTime);
    }

    List<List<List<float>>> GenerateRandomTheta(float miniumn, float maximum)
    {

        List<List<List<float>>> thetas = new List<List<List<float>>>();

        for (int i = 0; i < layers.Count; i++)
        {
            //for each layer. Initialize
            List<List<float>> layer = new List<List<float>>();

            for (int node = 0; node < layers[i]; node++)
            {

                List<float> weights = new List<float>();
                for (int weight = 0; weight < inputsDimension; weight++)
                {
                    weights.Add(Random.Range(-3f, 3f));
                }

                Debug.Log(weights);

                layer.Add(weights);
            }

            bestThetas.Add(layer);
        }

        return thetas;
    }


    

}
