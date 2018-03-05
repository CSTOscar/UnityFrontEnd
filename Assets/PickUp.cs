using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PickUp : MonoBehaviour {

    private GameObject player;
    private Detection detect;
    private bool mouse = false;
    private bool rayOn = false;
    private bool held = false;
    //private Quaternion original_rotation;
    //private Vector3 original_cam_rotation;

    public void setRayOn(bool b)
    {
        rayOn = b;
    }

    // Use this for initialization
    void Start () {
        player = GameObject.FindGameObjectWithTag("Player");
        detect = player.GetComponent<Detection>();
    }

    // Update is called once per frame
    void Update() {

        if (mouse == false)
        {
            mouse = Input.GetMouseButtonDown(0);
        }
        else
        {
            mouse = !Input.GetMouseButtonUp(0);
        }

        if (mouse == true && rayOn == true && !detect.getHolding())
        {
            held = true;
            detect.setHolding(true); //lock
            //original_rotation = gameObject.transform.rotation;
            //original_cam_rotation = GameObject.Find("Camera").transform.forward;
        }

        if (mouse == true && held) {
            if (gameObject.tag != "Knife")
            {
                gameObject.GetComponent<Collider>().enabled = false;
                gameObject.GetComponent<Rigidbody>().useGravity = false;
                gameObject.GetComponent<Rigidbody>().isKinematic = true;

                Transform cam = GameObject.Find("Camera").transform;
                gameObject.transform.position = cam.transform.position + cam.forward * 1;
                //gameObject.transform.rotation = original_rotation * Quaternion.FromToRotation(original_cam_rotation, new Vector3(cam.forward.x, cam.forward.y, original_cam_rotation.z));
            }
            else //if knife
            {
                gameObject.GetComponent<Rigidbody>().useGravity = false;
                gameObject.GetComponent<Rigidbody>().isKinematic = true;

                Transform cam = GameObject.Find("Camera").transform;
                gameObject.transform.position = cam.position + cam.forward * 0.7f + cam.up * -0.1f + cam.right * 0.2f;
                gameObject.transform.rotation = cam.rotation * Quaternion.Euler(210, 70, 100);
            }
        }
        else
        {
            if (held)
            {
                player.GetComponent<Detection>().setHolding(false); //release lock
                held = false;
            }
            rayOn = false;
            this.transform.parent = null;
            if (gameObject.tag != "Knife")
            {
                gameObject.GetComponent<Collider>().enabled = true;
            }
            this.GetComponent<Rigidbody>().useGravity = true;
            gameObject.GetComponent<Rigidbody>().isKinematic = false;
        }
	}
}
