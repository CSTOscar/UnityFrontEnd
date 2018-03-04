﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PickUp : MonoBehaviour {

    public Transform onHand; 
    private bool mouse;

	// Use this for initialization
	void Start () {
		onHand = GameObject.FindGameObjectWithTag("Player").transform;
    }
	
	// Update is called once per frame
	void Update () {
		if(mouse == true)
        {
            gameObject.GetComponent<Collider>().enabled = false;
            gameObject.GetComponent<Rigidbody>().useGravity = false;
            gameObject.GetComponent<Rigidbody>().isKinematic = true;
            Transform cam = GameObject.Find("Camera").transform;

            gameObject.transform.position = onHand.position + cam.forward*1;
            //this.transform.rotation = cam.rotation;

        }
        else
        {
            this.transform.parent = null;
            this.GetComponent<Collider>().enabled = true;
            this.GetComponent<Rigidbody>().useGravity = true;
            gameObject.GetComponent<Rigidbody>().isKinematic = false;
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
