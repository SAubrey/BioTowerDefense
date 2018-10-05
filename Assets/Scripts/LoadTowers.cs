using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

public class LoadTowers : MonoBehaviour
{

    public GameObject[] towers;
    private GameObject newInstance;

    // Use this for initialization
    void Start()
    {
        LoadAllTowers();   
    }

    // Update is called once per frame
    void Update()
    {
    }

    //Reloads towers, called by the DragAndDrop script attached to individual towers
    public void reloadTower(string name){
      
        GameObject t = Resources.Load("Prefabs/" + name) as GameObject;
        t = Instantiate(t, gameObject.transform, true);
        t.tag = "MenuItems";

    }   

    //Loads all towers on start
    void LoadAllTowers()
    {
        foreach (GameObject tow in towers)
        {

            GameObject t;
            t = Instantiate(tow, gameObject.transform, true);
            t.tag = "MenuItems";

        }
    }
}
