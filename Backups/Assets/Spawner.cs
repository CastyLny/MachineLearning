using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Spawner : MonoBehaviour
{
    public GameObject Food;
    public int initialFood = 10;
    public int foods = 0;
    public float spawnBound = 50f;
    public int growthRate = 2;
    public int Ceiling = 40;

    // Start is called before the first frame update
    void Start()
    {
        for (int i = 0; i < initialFood; i++)
        {
            Vector3 position = new Vector3(Random.Range(-1 * spawnBound, spawnBound), 2, Random.Range(-1 * spawnBound, spawnBound));
            Instantiate(Food, position, Quaternion.identity);
            foods += 1;

        }
        StartCoroutine("Iteration");
    }

    // Update is called once per frame
    void Update()
    {
        
    }


    IEnumerator Iteration()
    {
        while (true)
        {

            if (foods <= 0)
            {
                for (int i = 0; i < initialFood; i++)
                {
                    Vector3 position = new Vector3(Random.Range(-1 * spawnBound, spawnBound), 2, Random.Range(-1 * spawnBound, spawnBound));
                    Instantiate(Food, position, Quaternion.identity);
                    foods += 1;

                }
            }


            yield return new WaitForSeconds(Random.Range(0, 0.2f));
            int newFood = Ceiling - foods;
                //(growthRate * foods * (Ceiling - foods)) - foods;
            
            for (int i = 0; i < newFood; i++)
            {
                yield return new WaitForSeconds(Random.Range(0, 0.05f));
                Vector3 position = new Vector3(Random.Range(-1 * spawnBound, spawnBound), 2, Random.Range(-1 * spawnBound, spawnBound));
                Instantiate(Food, position, Quaternion.identity);

                foods++;
            }
        }
    }
}
