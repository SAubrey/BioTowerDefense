using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LoadTowers : MonoBehaviour
{

    public GameObject[] towers;

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
    //TODO: find way to initialize new tower without looping through entire array
    public void reloadTowers(){
          foreach (GameObject tow in towers)
          {
              if (transform.Find(tow.name + "(Clone)") == null)
              {
                 GameObject t;
                 t = Instantiate(tow, gameObject.transform, true);
                 t.tag = "MenuItems";

             }
         }
    
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
