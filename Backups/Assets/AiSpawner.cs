using System.Collections;
using System.Collections.Generic;
using System.IO;
using System;
using System.Text;
using UnityEngine.UI;
using UnityEngine;

public class AiSpawner : MonoBehaviour
{

    //Variables responsible for notingdown the history of everything
    List<float> costHis = new List<float>();
    List<float> bestSurvHis = new List<float>();



    public GameObject mobPrefab;
    public int MobNum;
    public Text generationsOutput;
    public Text CostOutput;
    public Text SurvivedTimeOutput;
    public Text bestThetaOutput1;
    public Text bestThetaOutput2;
    public Text bestThetaOutput3;

    public static bool export = false;

    public int generations;
    public int generation = 0;
    private bool StartNewGen = true;
    public List<float> surviveTimes = new List<float>();
    float bestSurvivedTime = 0f;

    public float alphaRate = 0.2f;


    public int inputsDimension = 5;

    public List<int> layers = new List<int>();

    public List<List<List<float>>> bestSpeedThetas = new List<List<List<float>>>();
    public List<List<List<float>>> bestXThetas = new List<List<List<float>>>();
    public List<List<List<float>>> bestZThetas = new List<List<List<float>>>();
    public List<List<List<float>>> bestXRotThetas = new List<List<List<float>>>();


    // Start is called before the first frame update
    void Start()
    {
        //Generate random values
        bestSpeedThetas = GenerateRandomTheta(-3f, 3f);
        bestXThetas = GenerateRandomTheta(-3f, 3f);
        bestZThetas = GenerateRandomTheta(-3f, 3f);
        bestXRotThetas = GenerateRandomTheta(-3f, 3f);

        CostOutput.text = "...";
        LogTheta(bestSpeedThetas, addSpace: true);
    }

    // Update is called once per frame
    void Update()
    {



        if (StartNewGen)
        {
            StartNewGen = false;
            generation++;
            Debug.Log(generation);
            generationsOutput.text = "Generation: " + generation.ToString();
            surviveTimes.Clear();

            SurvivedTimeOutput.text = "Spawning...";


            for (int i = 0; i < MobNum; i++)
            {

                Vector3 position = new Vector3(UnityEngine.Random.Range(-50f,50f), 2, UnityEngine.Random.Range(-50f,50f));
                GameObject mob = Instantiate(mobPrefab, position, Quaternion.identity);
                mob.transform.parent = this.transform;
                AI script = mob.GetComponent<AI>();
                script.SpeedThetas = AddThetas(bestSpeedThetas, GenerateRandomTheta(-alphaRate, alphaRate));
                script.XThetas = AddThetas(bestXThetas, GenerateRandomTheta(-alphaRate, alphaRate));
                script.ZThetas = AddThetas(bestZThetas, GenerateRandomTheta(-alphaRate, alphaRate));
                script.XRotThetas = AddThetas(bestXRotThetas, GenerateRandomTheta(-alphaRate, alphaRate));
            }
        }

        else if (MobNum <= surviveTimes.Count)
        {

            //Update and visualize thetas
            bestThetaOutput1.text = LogTheta(bestSpeedThetas);
            bestThetaOutput2.text = LogTheta(bestXThetas);
            bestThetaOutput3.text = LogTheta(bestZThetas);
            StartNewGen = true;

            //Update alphaRate
            alphaRate = Cost(bestSurvivedTime);
            

            //Update history
            costHis.Add(alphaRate);
            bestSurvHis.Add(bestSurvivedTime);

            CostOutput.text = "Cost: " + alphaRate.ToString();

            if (export)
            {
                string exportString = "Speed: " + LogTheta(bestSpeedThetas);
                exportString += ";XMovement: " + LogTheta(bestSpeedThetas);
                exportString += ";ZMovement: " + LogTheta(bestSpeedThetas);


                Export(exportString);
                export = false;

            }

        }
        else
        {
            
        }
    }

    //function that are called from the child object when it dies
    public void HandleDeath(float survivedTime, List<List<List<float>>> newXTheta, List<List<List<float>>> newZTheta, List<List<List<float>>> newSpeedTheta, List<List<List<float>>> newXRotTheta)
    {
        surviveTimes.Add(survivedTime);
        bestSurvivedTime = survivedTime;
        SurvivedTimeOutput.text = "Best survived Time: " + survivedTime.ToString();
        


        bestXThetas = newXTheta;
        bestZThetas = newZTheta;
        bestSpeedThetas = newSpeedTheta;
        bestXRotThetas = newXRotTheta;

    }

    List<List<List<float>>> GenerateRandomTheta(float minimum, float maximum)
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
                    weights.Add(UnityEngine.Random.Range(minimum, maximum));
                }


                //Debug.Log(weights.ToString());
                layer.Add(weights);
            }

            thetas.Add(layer);
        }

        Debug.Log(thetas.ToArray());
        return thetas;
    }

    List<List<List<float>>> GenerateUnifiedTheta(float value)
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
                    weights.Add(value);
                }

                //Debug.Log(weights);

                layer.Add(weights);
            }

            thetas.Add(layer);
        }

        return thetas;
    }




    List<List<List<float>>> AddThetas(List<List<List<float>>> first, List<List<List<float>>> second){//Function to add two theta together
        List<List<List<float>>> newTheta = new List<List<List<float>>>();

        for (int layerIndex = 0; layerIndex < first.Count; layerIndex++)
        {
            
            List<List<float>> layer = new List<List<float>>();

            for (int node = 0; node < first[layerIndex].Count; node++)
            {

                List<float> weights = new List<float>();
                for (int weight = 0; weight < first[layerIndex][node].Count; weight++)
                {

                    //Debug.Log(first[layerIndex][node][weight].ToString() + " " + second[layerIndex][node][weight].ToString());
                    weights.Add(first[layerIndex][node][weight] + second[layerIndex][node][weight]);
                }
                layer.Add(weights);
            }

            newTheta.Add(layer);
        }
        return newTheta;
    }

    float Cost(float value)
    {
        //Customized cost function for model
        return Mathf.Max(0,-Mathf.Log(value/200) + 1f);
    }


    string LogTheta(List<List<List<float>>> theta, bool addSpace = false)
    {
        string outputStr = "[";
        for (int layerIndex = 0; layerIndex < theta.Count; layerIndex++)
        {
            


            if (layerIndex != 0)
            {outputStr += ",";}
            outputStr += "[";
            

            for (int node = 0; node < theta[layerIndex].Count; node++)
            {
                if (node != 0)
                {
                    outputStr += ",";
                }
                outputStr += "[";
                for (int weight = 0; weight < theta[layerIndex][node].Count; weight++)
                {
                    if (weight != 0)
                    {
                        outputStr += ",";
                    }
                    outputStr += theta[layerIndex][node][weight].ToString("F1");

                }

                outputStr += "]";

                


            }

            if (addSpace)
            {
                outputStr += "\n";
            }
            outputStr += "]";
        }
      
        outputStr += "]";
        //Debug.Log(outputStr);
        return outputStr;
    }

    public void OnExport()
    {
        export = true;
    }

    void Export(string writeOver)
    {
        string fileName = @"C:\Users\Dell\Desktop\Simulation\Theta.txt";
        try
        {
            // Check if file already exists. If yes, delete it.     
            if (File.Exists(fileName))
            {
                File.Delete(fileName);
            }

            // Create a new file     
            using (FileStream fs = File.Create(fileName))
            {
                // Add some text to file    
                Byte[] title = new UTF8Encoding(true).GetBytes("Theta");
                Debug.Log("Hello Darkness my old friend");
                byte[] write = new UTF8Encoding(true).GetBytes(writeOver);
                fs.Write(write, 0, write.Length);
            }
        }
        catch (Exception Ex)
        {
            Console.WriteLine(Ex.ToString());
        }
    }


    

}
