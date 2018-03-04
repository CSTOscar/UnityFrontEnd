using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class KnifePickUp : MonoBehaviour
{

    public Transform onHand;
    private bool mouse;

    // Use this for initialization
    void Start()
    {
        onHand = GameObject.FindGameObjectWithTag("Player").transform;
    }

    // Update is called once per frame
    void Update()
    {
        if (mouse == true)
        {
            gameObject.GetComponent<Rigidbody>().useGravity = false;
            gameObject.GetComponent<Rigidbody>().isKinematic = true;
            Transform cam = GameObject.Find("Camera").transform;

            gameObject.transform.position = cam.position + cam.forward * 0.5f + new Vector3(0.3f, -0.3f, 0f);
            gameObject.transform.rotation = cam.rotation * Quaternion.Euler(210, 70, 100);
            

        }
        else
        {
            this.transform.parent = null;
            this.GetComponent<Rigidbody>().useGravity = true;
            gameObject.GetComponent<Rigidbody>().isKinematic = false;
        }
    }

    private void OnMouseDown()
    {
        mouse = true;
    }

    private void OnMouseUp()
    {
        mouse = false;
    }

}
