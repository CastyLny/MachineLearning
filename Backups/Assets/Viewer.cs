using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Viewer : MonoBehaviour
{
    public float speed = 10f;
    public float sensitivity = 50f;


    float xRotation = 0f;


    Camera main;
    public Camera secondary;
    public Camera third;

    int i = 0;

    // Start is called before the first frame update
    void Start()
    {
        main = Camera.main;
    }

    // Update is called once per frame
    void Update()
    {
        

        if (Input.GetKeyDown("space"))
        {
            i++;
            Debug.Log("Recevied");
            switch (i)
            {
                case 0:
                    
                    main.enabled = false;
                    secondary.enabled = true;
                    third.enabled = false;
                    break;
                case 1:
                    main.enabled = false;
                    secondary.enabled = false;
                    third.enabled = true;
                    break;
                case 2:
                    main.enabled = true;
                    secondary.enabled = false;
                    third.enabled = false;
                    i = -1;
                    break;

            
            }
        }
    }

    void Move(float speed)
    {
        this.transform.position += Vector3.forward * Input.GetAxis("Vertical") * speed * Time.deltaTime;
        //this.transform.position += Vector3.right * Time.deltaTime * Input.GetAxis("Horizontal") * speed;

        float MousePosX = Input.GetAxis("Mouse X") * Time.deltaTime * sensitivity;
        float MousePosY = Input.GetAxis("Mouse Y") * Time.deltaTime * sensitivity;

        xRotation -= MousePosX;
        this.transform.localRotation = Quaternion.Euler(xRotation, 0f, 0f);

        
        this.transform.Rotate(Vector3.right * MousePosY);





    }
}
