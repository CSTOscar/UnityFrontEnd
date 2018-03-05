using System.Collections;
using Newtonsoft.Json.Linq;
using Quobject.SocketIoClientDotNet.Client;
using System.Collections.Generic;
using System.Text.RegularExpressions;
using System.IO;
using System;
using UnityEngine;

//Connects to an online server to receive JSON data, parses,
//and generates virtual world from this data
public class ImportFurnitures : MonoBehaviour {

    WorldObject[] assetList;
    UnityEngine.Object[] instantiatedObjects;

    //JSON object that represents a virtual objects
    [System.Serializable]
    public class WorldObject {
        public List<float> position;
        public int id;
        public List<float> orientation;
        public float size;
    }

    //JSON object that represents a virtual wall
    [System.Serializable]
    public class Wall {
        public List<float> position1;
        public List<float> position2;
        public float height;
    }

    //boilerplating to convert an array of JSON objects into an array
    //of WorldObjects using the JsonUtility library
    public static class JsonHelper {
        public static T[] FromJson<T>(string json)
        {
            json = "{\"Items\":" + json + "}";
            Wrapper<T> wrapper = JsonUtility.FromJson<Wrapper<T>>(json);
            return wrapper.Items;
        }

        [System.Serializable]
        private class Wrapper<T>
        {
            public T[] Items;
        }
    }

    //converts a list of 2 floats to a Vector2
    Vector2 listToVec2(List<float> rawVec) {
        Vector2 pos = new Vector2(rawVec[0],rawVec[1]);
        return pos;
    }

    //converts a list of 3 floats to a Vector3
    Vector3 listToVec3(List<float> rawVec) {
        Vector3 pos = new Vector3(rawVec[0],-rawVec[1],rawVec[2]);
        return pos;
    }

    //converts degrees to radians
    public float ConvertToRadians(double angle) {
        return (float)((System.Math.PI / 180) * angle);
    }

    //converts radians to degrees
    public float ConvertToDegrees(double angle) {
        return (float)((angle * 180)/System.Math.PI);
    }

    //obtain virtual size of object from MeshRenderer, then use this to
    //rescale relatively to desired size
    public void newScale(GameObject obj, float newSize, char axis) {
        float size = 1;
        if (obj.tag == "Laptop") {
            size = 0.75F;
        } else {
            switch (axis) {
                case 'x':
                    size = (float)obj.GetComponent<Renderer> ().bounds.size.x;
                    break;
                case 'y':
                    size = (float)obj.GetComponent<Renderer> ().bounds.size.y;
                    break;
                case 'z':
                    size = (float)obj.GetComponent<Renderer> ().bounds.size.z;
                    break;
    			default:
    				size = (float)obj.GetComponent<Renderer> ().bounds.size.x;
    				break;
            }
        }
        Vector3 rescale = obj.transform.localScale;
        rescale = newSize * rescale / size;
        obj.transform.localScale = rescale;
    }

    //add plane to floor of world
    void spawnFloor() {
        GameObject floor = GameObject.CreatePrimitive(PrimitiveType.Plane);
        floor.transform.localScale = new Vector3(100, 100, 100);
        floor.transform.position = new Vector3(0,0,0);
    }

    //generate a wall from point A to point B, with height "height"
    void spawnWall(Vector2 A, Vector2 B, float height) {
        GameObject wall = GameObject.CreatePrimitive(PrimitiveType.Cube);
        double length = Vector2.Distance(A,B);
        double angle =  System.Math.Atan2(B[1] - A[1],B[0] - A[0]);
        wall.transform.localScale = new Vector3((float)length, height, 0.01F);
        wall.transform.Rotate(0,ConvertToDegrees(angle),0);
        wall.transform.position = new Vector3((A.x + B.x)/2,0,(A.y + B.y)/2);
    }

    //delete any existing objects in world
    int clearworld() {
        if (assetList != null) {
            for (int i = 0; i < assetList.Length; i++) {
                Destroy(instantiatedObjects[i]);
            }
        }
        return 0;
    }

    int generateWorld(string objectjson) {

        //reset world
        clearworld();

        //set object array to match json input
        assetList = JsonHelper.FromJson<WorldObject>(objectjson);
        //create array for actual Unity Objects in order to delete later
        instantiatedObjects = new UnityEngine.Object[assetList.Length];

        //match objects to correct models based on id tags
        //scale objects appropriately based on longest axis
        for (int i = 0; i < assetList.Length; i++) {
            GameObject obj;
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
            //position and rotate objects
            obj.transform.position = listToVec3(assetList[i].position);
            obj.transform.Rotate(0,0,ConvertToDegrees(assetList[i].orientation[0]));
            instantiatedObjects[i] = obj;
        }

        //unimplemented walls in backend

        //for (int i = 0; i < wallList.Length; i++) {
        //    spawnWall(listToVec2(wallList[i].position1), listToVec2(wallList[i].position2), wallList[i].height);
        //}

        return 0;
    }

    void Awake() {

        //opening connection to server and listening for data
        spawnFloor();

        IO.Options opts = new IO.Options();

        opts.Path = "/capture/socket.io";

        //open connection and notify server
        var socket = IO.Socket("https://jyy24.kings.cam.ac.uk", opts);
        socket.On(Socket.EVENT_CONNECT, () => {
            Debug.Log("connected");
            socket.Emit("recognitionRequest");
            });

        //on receipt of valid json, regenerate the world
        socket.On("recognised", (data)=>{
            var dummyObj = data as JToken;
            generateWorld(dummyObj.ToString());
        });

        //empty json file, debugging purposes
        socket.On("error", (data) =>{
            Debug.Log(data);
            Debug.Log(data.GetType());
        });

    }

    // Use this for initialization
    void Start () {

	}

	// Update is called once per frame
	void Update () {

	}
}
