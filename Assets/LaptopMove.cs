using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LaptopMove : MonoBehaviour
{
    public bool rotationPending;
    

    private bool isOpen;
    private Transform key;
    private Transform screen;
    private Quaternion localOpenAngle = new Quaternion(0.0f, 0.0f, 0.0f, 1.0f);
    private Quaternion localCloseAngle = new Quaternion(0.8f, 0.0f, 0.0f, 0.6f);
    private Vector3 eulerOpen = new Vector3(0.0f, 0.0f, 0.0f);
    private Vector3 eulerClosed = new Vector3(74.6f, 180.0f, 180.0f);
    //0 0 0
    //74.6 180 180

    // Use this for initialization
    void Start()
    {
        isOpen = true;
        key = this.gameObject.transform.Find("keyboard");
        screen = this.gameObject.transform.Find("screen");
        rotationPending = false;
    }

    // Update is called once per frame
    void Update()
    {
        if (rotationPending == true)
        {
            if (isOpen == true)
            {
                if (Vector3.Distance(screen.localRotation.eulerAngles, eulerClosed) > 1.0f)
                {
                    screen.Rotate(Vector3.right * 100 * Time.deltaTime);
                }
                else
                {
                    screen.localEulerAngles = eulerClosed;
                    isOpen = false;
                    rotationPending = false;
                }
            }
            else
            {
                if (Vector3.Distance(screen.localRotation.eulerAngles, eulerOpen) > 3.0f)
                {
                    screen.Rotate(-Vector3.right * 100 * Time.deltaTime);
                }
                else
                {
                    screen.localEulerAngles = eulerOpen;
                    isOpen = true;
                    rotationPending = false;
                }
            }
        }
        
        //Debug
        /*
        if (Input.GetKey(KeyCode.A))
        {
            screen.Rotate(Vector3.right * 30 * Time.deltaTime);
        }
        if (Input.GetKey(KeyCode.D))
        {
            screen.transform.Rotate(-Vector3.right * 30 * Time.deltaTime);
        }
        if (Input.GetKey(KeyCode.S))
        {
            Debug.Log(screen.localRotation);
            Debug.Log(screen.localRotation.eulerAngles);
        }
        */
    }

    public void Move()
    {
        rotationPending = true;
    }
}