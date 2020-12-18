using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class food : MonoBehaviour
{

    public GameObject spawner;
    public float LifeRange = 5f;
    private float dyingTime;
    

    // Start is called before the first frame update
    void Start()
    {
        spawner = GameObject.Find("SpawnSys");
        dyingTime = Random.Range(2f, LifeRange);

        StartCoroutine("circle");
    }

    IEnumerator circle()
    {
        Spawner script = spawner.GetComponent<Spawner>();
        yield return new WaitForSeconds(dyingTime);
        script.foods -= 1;
        Destroy(this.gameObject);
    }


    public void die()
    {
        Spawner script = spawner.GetComponent<Spawner>();
        script.foods -= 1;
        Destroy(this.gameObject);
    }
}
