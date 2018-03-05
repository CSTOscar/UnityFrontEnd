using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FruitCut : MonoBehaviour {

    private float timeTouched;
    private bool secondTouch = false;

	// Use this for initialization
	void Start () {
        timeTouched = Time.time;
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
                if(secondTouch == false)
                {
                    secondTouch = true;
                    GameObject.Instantiate((GameObject)Resources.Load("AppleHalf"), gameObject.transform.position, gameObject.transform.rotation);
                    Destroy(gameObject);
                }

            }
            else if(gameObject.tag == "Banana")
            {
                if(secondTouch == false)
                {
                    secondTouch = true;
                    GameObject.Instantiate((GameObject)Resources.Load("BananaOpen"), gameObject.transform.position, gameObject.transform.rotation);
                    Destroy(gameObject);
                }

            }
            else if(gameObject.tag == "BananaOpen")
            {
                if (timeTouched + 1.0f < Time.time && secondTouch == false)
                {
                    secondTouch = true;
                    GameObject.Instantiate((GameObject)Resources.Load("BananaPeeled"), gameObject.transform.position, gameObject.transform.rotation);
                    Destroy(gameObject);
                }
            }
        }
    }
}
