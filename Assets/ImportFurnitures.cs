using System.Collections;
using System.Collections.Generic;
using System.Text.RegularExpressions;
using System.Web;
using System.IO;
using UnityEngine;

public class ImportFurnitures : MonoBehaviour {

    public TextAsset data;

    /*[System.Serializable]
    public class WorldObject {
        public List<double> position { get; set; }
        public List<List<double>> precision { get; set; }
        public int @class { get; set; }
        public List<double> direction { get; set; }
        public double size { get; set; }
    }*/

    [System.Serializable]
    public class WorldObject {
        public List<int> position;
        public int id;
        public List<double> orientation;
        public int size;
    }

    [System.Serializable]
    public class Wall {
        public List<int> position1;
        public List<int> position2;
        public int height;
    }

    public static class JsonHelper {
        public static T[] FromJson<T>(string json)
        {
            json = "{\"Items\":" + json + "}";
            Wrapper<T> wrapper = JsonUtility.FromJson<Wrapper<T>>(json);
            return wrapper.Items;
        }

        public static string ToJson<T>(T[] array)
        {
            Wrapper<T> wrapper = new Wrapper<T>();
            wrapper.Items = array;
            return JsonUtility.ToJson(wrapper);
        }

        public static string ToJson<T>(T[] array, bool prettyPrint)
        {
            Wrapper<T> wrapper = new Wrapper<T>();
            wrapper.Items = array;
            return JsonUtility.ToJson(wrapper, prettyPrint);
        }

        [System.Serializable]
        private class Wrapper<T>
        {
            public T[] Items;
        }
    }

    Vector3 rawToVec3(string[] rawVec) {
        Vector3 pos = new Vector3(float.Parse(rawVec[0]), float.Parse(rawVec[1]), float.Parse(rawVec[2]));
        return pos;
    }

    Vector2 listToVec2(List<int> rawVec) {
        Vector2 pos = new Vector2((float)rawVec[0],(float)rawVec[1]);
        return pos;
    }

    Vector3 listToVec3(List<int> rawVec) {
        Vector3 pos = new Vector3((float)rawVec[0],(float)rawVec[1],(float)rawVec[2]);
        return pos;
    }

    public float ConvertToRadians(double angle) {
        return (float)((System.Math.PI / 180) * angle);
    }

    public float ConvertToDegrees(double angle) {
        return (float)((angle * 180)/System.Math.PI);
    }

    public void newScale(GameObject theGameObject, float newSize, char axis) {
        float size = 0;
        switch (axis) {
            case 'x':
                size = theGameObject.GetComponent<Renderer> ().bounds.size.x;
                break;
            case 'y':
                size = theGameObject.GetComponent<Renderer> ().bounds.size.y;
                break;
            case 'z':
                size = theGameObject.GetComponent<Renderer> ().bounds.size.z;
                break;
        }
        Vector3 rescale = theGameObject.transform.localScale;
        rescale = newSize * rescale / size;
        theGameObject.transform.localScale = rescale;
    }

    void spawnFloor() {
        GameObject floor = GameObject.CreatePrimitive(PrimitiveType.Plane);
        floor.transform.localScale = new Vector3(100, 100, 100);
        floor.transform.position = new Vector3(0,0,0);
    }

    void spawnWall(Vector2 A, Vector2 B, int height) {
        //Generate an infinitely tall wall from point A to point B
        GameObject wall = GameObject.CreatePrimitive(PrimitiveType.Cube);
        double length = Vector2.Distance(A,B);
        double angle =  System.Math.Atan2(B[1] - A[1],B[0] - A[0]);
        wall.transform.localScale = new Vector3((float)length, (float)height, 0.01F);
        wall.transform.Rotate(0,ConvertToDegrees(angle),0);
        wall.transform.position = new Vector3((A.x + B.x)/2,0,(A.y + B.y)/2);
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
        //string raw = data.text;
        spawnFloor();
        string json = File.ReadAllText("object_data.txt");
        WorldObject[] assetList = JsonHelper.FromJson<WorldObject>(json);
        json = File.ReadAllText("wall_data.txt");
        Wall[] wallList = JsonHelper.FromJson<Wall>(json);

        Debug.Log(assetList.Length);

        for (int i = 0; i < assetList.Length; i++) {
            Debug.Log((float)assetList[i].orientation[0]);
        }

        for (int i = 0; i < assetList.Length; i++) {
            GameObject obj;// = GameObject.CreatePrimitive(PrimitiveType.Sphere);
            switch (assetList[i].id) {
                //door
                case 2:
                    obj = GameObject.Instantiate((GameObject)Resources.Load("DoorPrefab"));
                    newScale(obj, assetList[i].size, 'z');
                    break;
                //bottle
                case 44:
                    obj = GameObject.Instantiate((GameObject)Resources.Load("Bottle"));
                    newScale(obj, assetList[i].size, 'x');
                    break;
                //mug
                case 47:
                    obj = GameObject.Instantiate((GameObject)Resources.Load("mug"));
                    newScale(obj, assetList[i].size, 'x');
                    break;
                //fork
                case 48:
                    obj = GameObject.Instantiate((GameObject)Resources.Load("Fork"));
                    newScale(obj, assetList[i].size, 'y');
                    break;
                //knife
                case 49:
                    obj = GameObject.Instantiate((GameObject)Resources.Load("Knife"));
                    newScale(obj, assetList[i].size, 'y');
                    break;
                //spoon
                case 50:
                    obj = GameObject.Instantiate((GameObject)Resources.Load("Spoon"));
                    newScale(obj, assetList[i].size, 'y');
                    break;
                //bowl
                case 51:
                    obj = GameObject.Instantiate((GameObject)Resources.Load("bowl"));
                    newScale(obj, assetList[i].size, 'x');
                    break;
                //banana
                case 52:
                    obj = GameObject.Instantiate((GameObject)Resources.Load("Banana"));
                    newScale(obj, assetList[i].size, 'y');
                    break;
                //apple
                case 53:
                    obj = GameObject.Instantiate((GameObject)Resources.Load("Apple"));
                    newScale(obj, assetList[i].size, 'x');
                    break;
                //sandwich
                case 54:
                    obj = GameObject.Instantiate((GameObject)Resources.Load("Sandwich"));
                    newScale(obj, assetList[i].size, 'x');
                    break;
                //orange
                case 55:
                    obj = GameObject.Instantiate((GameObject)Resources.Load("Orange"));
                    newScale(obj, assetList[i].size, 'x');
                    break;
                //chair
                case 62:
                    obj = GameObject.Instantiate((GameObject)Resources.Load("Chair_1"));
                    newScale(obj, assetList[i].size, 'x');
                    break;
                //table
                case 67:
                    obj = GameObject.Instantiate((GameObject)Resources.Load("CoffeTable_1"));
                    newScale(obj, assetList[i].size, 'x');
                    break;
                //tv
                case 72:
                    obj = GameObject.Instantiate((GameObject)Resources.Load("FlatScreenTV"));
                    newScale(obj, assetList[i].size, 'x');
                    break;
                //laptop
                case 73:
                    obj = GameObject.Instantiate((GameObject)Resources.Load("laptop"));
                    newScale(obj, assetList[i].size, 'x');
                    break;
                //phone
                case 77:
                    obj = GameObject.Instantiate((GameObject)Resources.Load("smartphone"));
                    newScale(obj, assetList[i].size, 'y');
                    break;
                //toaster
                case 80:
                    obj = GameObject.Instantiate((GameObject)Resources.Load("toaster"));
                    newScale(obj, assetList[i].size, 'x');
                    break;
                //book
                case 84:
                    obj = GameObject.Instantiate((GameObject)Resources.Load("Books"));
                    newScale(obj, assetList[i].size, 'x');
                    break;
                //scissors
                case 87:
                    obj = GameObject.Instantiate((GameObject)Resources.Load("Scissors_blue"));
                    newScale(obj, assetList[i].size, 'y');
                    break;
                //error
                default:
                    obj = GameObject.CreatePrimitive(PrimitiveType.Cube);
                    Debug.Log("object " + assetList[i].id + " not found");
                    break;
            }
            obj.transform.position = listToVec3(assetList[i].position);
            obj.transform.Rotate(0,0,ConvertToDegrees(assetList[i].orientation[0]));
        }

        for (int i = 0; i < wallList.Length; i++) {
            spawnWall(listToVec2(wallList[i].position1), listToVec2(wallList[i].position2), wallList[i].height);
        }
        //}
        /*//
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
            *
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
        }*/
    }

    // Use this for initialization
    void Start () {

	}

	// Update is called once per frame
	void Update () {

	}
}
