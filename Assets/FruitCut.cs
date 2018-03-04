using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FruitCut : MonoBehaviour {

    private float timeBananaPeeled;

	// Use this for initialization
	void Start () {
        timeBananaPeeled = Time.time;
	}
	
	// Update is called once per frame
	void Update () {
		
	}

    private void OnCollisionEnter(Collision collision)
    {
        if(collision.collider.tag == "Knife")
        {
            if(gameObject.tag == "Apple")
            {
                GameObject.Instantiate((GameObject)Resources.Load("AppleHalf"), gameObject.transform.position, gameObject.transform.rotation);
                Destroy(gameObject);
            }
            else if(gameObject.tag == "Banana")
            {
                GameObject.Instantiate((GameObject)Resources.Load("BananaOpen"), gameObject.transform.position, gameObject.transform.rotation);
                Destroy(gameObject);
            }
            else if(gameObject.tag == "BananaOpen")
            {
                Debug.Log(timeBananaPeeled + "  " + Time.time);
                if(timeBananaPeeled + 1.0f < Time.time)
                {
                    GameObject.Instantiate((GameObject)Resources.Load("BananaPeeled"), gameObject.transform.position, gameObject.transform.rotation);
                    Destroy(gameObject);
                }
            }
        }
    }
}
