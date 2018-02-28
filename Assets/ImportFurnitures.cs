using System.Collections;
using System.Collections.Generic;
using System.Text.RegularExpressions;
using UnityEngine;

public class ImportFurnitures : MonoBehaviour {

    public TextAsset data;

    void Awake(){

        //loading data
        string raw = data.text;
        //
        string[] lines = Regex.Split(raw, "\r\n|\n|\r");

        string readState = "none";
        foreach (string line in lines)
        {
            Debug.Log(line);
            //every odd lines have furniture labels, this if part identifies it
            if (readState == "none")
            {
                if(line == "door")
                {
                    readState = line;
                }
                else
                {
                    throw new System.Exception("data with wrong furniture name");
                }
            }
            /* now having read the label from the odd line, the info about the furniture 
             * is in these even lines and are handled depending on the label
            */
            else
            {
                if(readState =="door")
                {
                    //door's features look like
                    //position(x,y,z)/bearing(wrt y axis/hingeSide
                    string[] features = line.Split('/');
                    string[] rawVec = features[0].Split(',');
                    Vector3 pos = new Vector3(float.Parse(rawVec[0]), float.Parse(rawVec[1]), float.Parse(rawVec[2]));
                    float bearing = float.Parse(features[1]);
                    string side = features[2];

                    //having parsed the features, let's instantiate a door
                    GameObject door = GameObject.Find("DummyDoor");
                    Debug.Log(pos);
                    Debug.Log(bearing);
                    Debug.Log(side);
                    GameObject newDoor = Instantiate(door, pos, Quaternion.Euler(0, bearing, 0));
                    Door doorProp = newDoor.GetComponent<Door>();
                    if(side == "l")
                    {
                        doorProp.RotationSide = Door.SideOfRotation.Left;
                    }
                    else
                    {
                        doorProp.RotationSide = Door.SideOfRotation.Right;
                    }

                    //re-invisiblize it
                    newDoor.GetComponent<Invisible>().enabled = false;
                }
                else
                {
                    //use else if to read other furnitures
                }
                readState = "none"; //now set the state to read odd lines again
            }
        }
    }

    // Use this for initialization
    void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		
	}
}
               