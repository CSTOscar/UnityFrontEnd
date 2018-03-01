using System.Collections;
using System.Collections.Generic;
using System.Text.RegularExpressions;
using UnityEngine;

public class ImportFurnitures : MonoBehaviour {

    public TextAsset data;

    Vector3 rawToVec3(string[] rawVec) {
        Vector3 pos = new Vector3(float.Parse(rawVec[0]), float.Parse(rawVec[1]), float.Parse(rawVec[2]));
        return pos;
    }

    void Room(Vector3 centre, Vector3 dimensions) {
		//generate 6 walls of a room
		GameObject[] walls = new GameObject[6];
		for (int i = 0; i < walls.Length; i++) {
			walls[i] = GameObject.CreatePrimitive (PrimitiveType.Cube);
		}

		//scale walls appropriately
		walls[0].transform.localScale = new Vector3 (dimensions.x/1, 0.05F, dimensions.y/1);
		walls[1].transform.localScale = new Vector3 (dimensions.x/1, 0.05F, dimensions.y/1);
		walls[2].transform.localScale = new Vector3 (dimensions.x/1, 0.05F, dimensions.z/1);
		walls[3].transform.localScale = new Vector3 (dimensions.x/1, 0.05F, dimensions.z/1);
		walls[4].transform.localScale = new Vector3 (dimensions.y/1, 0.05F, dimensions.z/1);
		walls[5].transform.localScale = new Vector3 (dimensions.y/1, 0.05F, dimensions.z/1);

		//rotate walls appropriately (2 and 3 are already in corredct orientation)
		walls[0].transform.Rotate(90,0,0);
		walls[1].transform.Rotate(90,0,0);
		walls[4].transform.Rotate(0,0,90);
		walls[5].transform.Rotate(0,0,90);

		//translate walls appropriately
		walls[0].transform.position = new Vector3(centre.x,centre.y,centre.z - dimensions.z/2);
		walls[1].transform.position = new Vector3(centre.x,centre.y,centre.z + dimensions.z/2);
		walls[2].transform.position = new Vector3(centre.x,centre.y - dimensions.y/2,centre.z);
		walls[3].transform.position = new Vector3(centre.x,centre.y + dimensions.y/2,centre.z);
		walls[4].transform.position = new Vector3(centre.x - dimensions.x/2,centre.y,centre.z);
		walls[5].transform.position = new Vector3(centre.x + dimensions.x/2,centre.y,centre.z);
	}

    void Awake() {

        //loading data
        string raw = data.text;
        //
        string[] lines = Regex.Split(raw, "\r\n|\n|\r");

        string readState = "none";
        foreach (string line in lines) {
            Debug.Log(line);
            //every odd lines have furniture labels, this if part identifies it
            if (readState == "none") {
                if (line == "door" | line == "chair" | line == "table" | line == "room") {
                    readState = line;
                }
                else {
                    throw new System.Exception("data with wrong furniture name");
                }
            }
            /* now having read the label from the odd line, the info about the furniture
             * is in these even lines and are handled depending on the label
            */
            else
            {
                //string[] features = line.Split('/');
                //string[] rawVec = features[0].Split(',');
                //float bearing = float.Parse(features[1]);

                if (readState == "door") {
                    //door's features look like
                    //position(x,y,z)/bearing(wrt y axis/hingeSide
                    string[] features = line.Split('/');
                    string[] rawVec = features[0].Split(',');
                    Vector3 pos = new Vector3(float.Parse(rawVec[0]), float.Parse(rawVec[1]), float.Parse(rawVec[2]));
                    float bearing = float.Parse(features[1]);
                    string side = features[2];

                    //having parsed the features, let's instantiate a door
                    //GameObject door = GameObject.Find("DummyDoor");
                    GameObject door = GameObject.Instantiate((GameObject)Resources.Load("DoorPrefab"));
                    Debug.Log(pos);
                    Debug.Log(bearing);
                    Debug.Log(side);
                    door.transform.position = pos;
                    door.transform.Rotate(0,bearing,0);
                    Door doorProp = door.GetComponent<Door>();
                    //GameObject newDoor = Instantiate(door, pos, Quaternion.Euler(0, bearing, 0));
                    //Door doorProp = newDoor.GetComponent<Door>();
                    if (side == "l") {
                        doorProp.RotationSide = Door.SideOfRotation.Left;
                    }
                    else {
                        doorProp.RotationSide = Door.SideOfRotation.Right;
                    }

                    //re-invisiblize it
                    //newDoor.GetComponent<Invisible>().enabled = false;
                }
                else if (readState == "chair") {
                    string[] features = line.Split('/');
                    string[] rawVec = features[0].Split(',');
                    float bearing = float.Parse(features[1]);

                    //having parsed the features, let's instantiate a door
                    //GameObject door = GameObject.Find("DummyDoor");
                    GameObject obj = GameObject.Instantiate((GameObject)Resources.Load("Chair_1"));
                    obj.transform.position = rawToVec3(rawVec);
                    obj.transform.Rotate(0,0,0);

                    //re-invisiblize it
                }
                else if (readState == "table") {
                    string[] features = line.Split('/');
                    string[] rawVec = features[0].Split(',');
                    float bearing = float.Parse(features[1]);

                    //having parsed the features, let's instantiate a door
                    //GameObject door = GameObject.Find("DummyDoor");
                    GameObject obj = GameObject.Instantiate((GameObject)Resources.Load("CoffeTable_1"));
                    obj.transform.position = rawToVec3(rawVec);
                    obj.transform.Rotate(0,0,0);
                }
                else if (readState == "room") {
                    string[] features = line.Split('/');
                    string[] rawCentre = features[0].Split(',');
                    string[] rawDimensions = features[1].Split(',');
                    Room(rawToVec3(rawCentre),rawToVec3(rawDimensions));
                }
                else {

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
