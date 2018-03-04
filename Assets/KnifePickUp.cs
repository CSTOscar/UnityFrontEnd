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
            Debug.Log("reachesthisbit");
            gameObject.GetComponent<Rigidbody>().useGravity = false;
            Transform cam = GameObject.Find("Camera").transform;

            gameObject.transform.position = onHand.position + cam.forward * 1;
            //this.transform.rotation = cam.rotation;

        }
        else
        {
            this.transform.parent = null;
            this.GetComponent<Rigidbody>().useGravity = true;
        }
    }

    private void OnMouseDown()
    {
        mouse = true;
        Debug.Log(gameObject.name);

    }

    private void OnMouseUp()
    {
        mouse = false;
    }

}
