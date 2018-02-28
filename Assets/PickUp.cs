using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PickUp : MonoBehaviour {

    public Transform onHand;
    private bool mouse;

	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		if(mouse == true)
        {
            this.GetComponent<BoxCollider>().enabled = false;
            this.GetComponent<Rigidbody>().useGravity = false;
            Transform cam = GameObject.Find("Camera").transform;

            this.transform.position = onHand.position + cam.forward*1;
            //this.transform.rotation = cam.rotation;

        }
        else
        {
            this.transform.parent = null;
            this.GetComponent<BoxCollider>().enabled = true;
            this.GetComponent<Rigidbody>().useGravity = true;
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
