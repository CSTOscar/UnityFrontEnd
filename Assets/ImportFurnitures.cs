using System.Collections;
using System.Collections.Generic;
using System.Text.RegularExpressions;
using System.IO;
using System;
using UnityEngine;

public class ImportFurnitures : MonoBehaviour {

    public class Constants {
        public const string HOST = "192.x.x.x";
        public const int PORT = 9999;
    }

    [System.Serializable]
    public class WorldObject {
        public List<float> position;
        public int id;
        public List<float> orientation;
        public float size;
    }

    [System.Serializable]
    public class Wall {
        public List<float> position1;
        public List<float> position2;
        public float height;
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

    Vector2 listToVec2(List<float> rawVec) {
        Vector2 pos = new Vector2(rawVec[0],rawVec[1]);
        return pos;
    }

    Vector3 listToVec3(List<float> rawVec) {
        Vector3 pos = new Vector3(rawVec[0],rawVec[1],rawVec[2]);
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

    void spawnWall(Vector2 A, Vector2 B, float height) {
        GameObject wall = GameObject.CreatePrimitive(PrimitiveType.Cube);
        double length = Vector2.Distance(A,B);
        double angle =  System.Math.Atan2(B[1] - A[1],B[0] - A[0]);
        wall.transform.localScale = new Vector3((float)length, height, 0.01F);
        wall.transform.Rotate(0,ConvertToDegrees(angle),0);
        wall.transform.position = new Vector3((A.x + B.x)/2,0,(A.y + B.y)/2);
    }

    private static string GetArg(string name) {
        var args = System.Environment.GetCommandLineArgs();
        for (int i = 0; i < args.Length; i++) {
            if (args[i] == name && args.Length > i + 1) {
                return args[i + 1];
            }
        }
        return null;
    }

    void Awake() {

        //loading data
        spawnFloor();


        string json = File.ReadAllText("../AbsoluteObject3DMap/data/temp_files/results/object_data.txt");
        WorldObject[] assetList = JsonHelper.FromJson<WorldObject>(json);

        json = File.ReadAllText("../AbsoluteObject3DMap/data/temp_files/results/wall_data.txt");
        Wall[] wallList = JsonHelper.FromJson<Wall>(json);


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
            obj.transform.position = listToVec3(assetList[i].position);
            obj.transform.Rotate(0,0,ConvertToDegrees(assetList[i].orientation[0]));
        }

        for (int i = 0; i < wallList.Length; i++) {
            spawnWall(listToVec2(wallList[i].position1), listToVec2(wallList[i].position2), wallList[i].height);
        }
    }

    // Use this for initialization
    void Start () {

	}

	// Update is called once per frame
	void Update () {

	}
}

