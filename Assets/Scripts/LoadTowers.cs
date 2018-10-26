using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
using UnityEngine.UI;

public class LoadTowers : MonoBehaviour
{
        public Sprite test;
        public static Sprite amoxSprite;
        public static Sprite methSprite;
        public static Sprite vancSprite;
        public static Sprite carbSprite;
        public static Sprite lineSprite;
        public static Sprite rifaSprite;
        public static Sprite isonSprite;
        public static GameObject projectile;


    public static IDictionary<string, object> amoxTower = new Dictionary<string, object>(){
                                {"name", "Amoxicillin"},
                                {"towerPositionX", 8f},
                                {"towerPositionY", 3f},
                                {"cost", 10},
                                {"towerSprite", amoxSprite},
                                {"towerColor", Color.gray},
                                {"radius", 4f},
                                {"antibioticType", "amox"},
                                {"coolDown", 120.0f},
                                {"specialEffect", 0},
                                {"projectileType", 0},
                                {"projectileSize", 0.5f},
                                {"projectileSpeed", 0.6f},
                                {"projectileAOE", 0f},
                                {"projectileSprite", projectile},
                                {"projectilePierce", false},
                                {"targetType", 0}
    };


    public static IDictionary<string, object> methTower = new Dictionary<string, object>(){
                                {"name", "Methicillin"},
                                {"towerPositionX", 8f},
                                {"towerPositionY", 2f},
                                {"cost", 20},
                                {"towerSprite", methSprite},
                                {"towerColor", Color.magenta},
                                {"radius", 4f},
                                {"antibioticType", "meth"},
                                {"coolDown", 30.0f},
                                {"specialEffect", 0},
                                {"projectileType", 0},
                                {"projectileSize", 0.5f},
                                {"projectileSpeed", 0.6f},
                                {"projectileAOE", 0f},
                                {"projectileSprite", projectile},
                                {"projectilePierce", false},
                                {"targetType", 0}
    };


    public static IDictionary<string, object> vancTower = new Dictionary<string, object>(){
                                {"name", "Vancocymin"},
                                {"towerPositionX", 8f},
                                {"towerPositionY", 1f},
                                {"cost", 30},
                                {"towerSprite", vancSprite},
                                {"towerColor", Color.blue},
                                {"radius", 4f},
                                {"antibioticType", "vanc"},
                                {"coolDown", 30.0f},
                                {"specialEffect", 0},
                                {"projectileType", 0},
                                {"projectileSize", 0.5f},
                                {"projectileSpeed", 0.6f},
                                {"projectileAOE", 0f},
                                {"projectileSprite", projectile},
                                {"projectilePierce", false},
                                {"targetType", 0}
    };


    public static IDictionary<string, object> carbTower = new Dictionary<string, object>(){
                                {"name", "Carbapenem"},
                                {"towerPositionX", 8f},
                                {"towerPositionY", 0f},
                                {"cost", 40},
                                {"towerSprite", carbSprite},
                                {"towerColor", Color.yellow},
                                {"radius", 4f},
                                {"antibioticType", "carb"},
                                {"coolDown", 120.0f},
                                {"specialEffect", 0},
                                {"projectileType", 0},
                                {"projectileSize", 0.5f},
                                {"projectileSpeed", 0.6f},
                                {"projectileAOE", 0f},
                                {"projectileSprite", projectile},
                                {"projectilePierce", false},
                                {"targetType", 0}
    };


    public static IDictionary<string, object> lineTower = new Dictionary<string, object>(){
                                {"name", "Linezolid"},
                                {"towerPositionX", 8f},
                                {"towerPositionY", -1f},
                                {"cost", 50},
                                {"towerSprite", lineSprite},
                                {"towerColor", Color.green},
                                {"radius", 4f},
                                {"antibioticType", "line"},
                                {"coolDown", 30.0f},
                                {"specialEffect", 0},
                                {"projectileType", 0},
                                {"projectileSize", 0.5f},
                                {"projectileSpeed", 0.6f},
                                {"projectileAOE", 0f},
                                {"projectileSprite", projectile},
                                {"projectilePierce", false},
                                {"targetType", 0}
    };


    public static IDictionary<string, object> rifaTower = new Dictionary<string, object>(){
                                {"name", "Rifampicin"},
                                {"towerPositionX", 8f},
                                {"towerPositionY", -2f},
                                {"cost", 60},
                                {"towerSprite", rifaSprite},
                                {"towerColor", Color.red},
                                {"radius", 4f},
                                {"antibioticType", "rifa"},
                                {"coolDown", 30.0f},
                                {"specialEffect", 0},
                                {"projectileType", 0},
                                {"projectileSize", 0.5f},
                                {"projectileSpeed", 0.6f},
                                {"projectileAOE", 0f},
                                {"projectileSprite", projectile},
                                {"projectilePierce", false},
                                {"targetType", 0}
    };

    public static IDictionary<string, object> isonTower = new Dictionary<string, object>(){
                                {"name", "Isoniazid"},
                                {"towerPositionX", 8f},
                                {"towerPositionY", -3f},
                                {"cost", 70},
                                {"towerSprite", isonSprite},
                                {"towerColor", Color.cyan},
                                {"radius", 4f},
                                {"antibioticType", "ison"},
                                {"coolDown", 30.0f},
                                {"specialEffect", 0},
                                {"projectileType", 0},
                                {"projectileSize", 0.5f},
                                {"projectileSpeed", 0.6f},
                                {"projectileAOE", 0f},
                                {"projectileSprite", projectile},
                                {"projectilePierce", false},
                                {"targetType", 0}
    };

    public IDictionary<string, IDictionary<string, object>> towers = new Dictionary<string, IDictionary<string, object>>(){
                                    {"Amoxicillin", amoxTower},
                                    {"Methicillin", methTower},
                                    {"Vancocymin", vancTower},
                                   {"Carbapenem", carbTower},
                                   {"Linezolid", lineTower},
                                   {"Rifampicin", rifaTower},
                                   {"Isoniazid", isonTower} 
    };


    private GameObject tower;

    private __app appTowers;

    // Use this for initialization
    void Start()
    {
        appTowers = GameObject.Find("__app").GetComponent<__app>();
        tower = Resources.Load("Prefabs/Tower") as GameObject;

        //Load the towers sprites, and assign them to their spots in the dictionaries
        towers["Amoxicillin"]["towerSprite"] = Resources.Load<Sprite>("Sprites/Towers/amoxSprite") as Sprite;
        towers["Methicillin"]["towerSprite"] = Resources.Load<Sprite>("Sprites/Towers/methSprite") as Sprite;
        towers["Vancocymin"]["towerSprite"] = Resources.Load<Sprite>("Sprites/Towers/vancSprite") as Sprite;
        towers["Carbapenem"]["towerSprite"] = Resources.Load<Sprite>("Sprites/Towers/carbSprite") as Sprite;
        towers["Linezolid"]["towerSprite"] = Resources.Load<Sprite>("Sprites/Towers/lineSprite") as Sprite;
        towers["Rifampicin"]["towerSprite"] = Resources.Load<Sprite>("Sprites/Towers/rifaSprite") as Sprite;
        towers["Isoniazid"]["towerSprite"] = Resources.Load<Sprite>("Sprites/Towers/isonSprite") as Sprite;

        projectile = Resources.Load("Prefabs/Projectile") as GameObject;

        LoadAllTowers();

    }

    // Update is called once per frame
    void Update()
    {
    }

    //Reloads towers, called by the DragAndDrop script attached to individual towers
    public void reloadTower(string towerName){
      
        IDictionary <string, object> towerAttributes;
        towerAttributes = towers[towerName];

        GameObject newTower = setTowerAttributes(towerAttributes);
        Instantiate(newTower, gameObject.transform, true);

    }

    //Loads all towers on start
    void LoadAllTowers()
    {

        foreach (KeyValuePair<string, IDictionary<string, object>> tow in towers)
        {     
            GameObject newTower = setTowerAttributes(tow.Value);
            Instantiate(newTower, gameObject.transform, true);
        }
    }

    //Set the towers attributes before spawning to the menu
    GameObject setTowerAttributes(IDictionary<string, object> attributes){

            GameObject t;
            t = tower;

            t.GetComponent<Tower>().coolDown = (float)attributes["coolDown"];
            t.GetComponent<Tower>().targetType = (int) attributes["targetType"];
            t.GetComponent<Tower>().projectileType = (int) attributes["projectileType"];
            t.GetComponent<Tower>().antibioticType = (string) attributes["antibioticType"];
            t.GetComponent<Tower>().projectileSize = (float)attributes["projectileSize"];
            t.GetComponent<Tower>().projectileSpeed = (float)attributes["projectileSpeed"];
            t.GetComponent<Tower>().projectileAOE = (float)attributes["projectileAOE"];
            t.GetComponent<Tower>().projectileSprite = null;
            t.GetComponent<Tower>().projectilePierce = (bool) attributes["projectilePierce"];
            t.GetComponent<Tower>().detectionRadius = (float)attributes["radius"];
            t.GetComponent<Tower>().specialEffect = (int) attributes["specialEffect"];
            t.GetComponent<Tower>().towerCost = (int) attributes["cost"];
            t.GetComponent<Tower>().towerName = (string) attributes["name"];
            t.GetComponent<SpriteRenderer>().sprite = (Sprite) attributes["towerSprite"];
            t.GetComponent<SpriteRenderer>().color = (Color) attributes["towerColor"];

            float xPos =  (float) attributes["towerPositionX"];
            float yPos =  (float) attributes["towerPositionY"];

            t.transform.position = new Vector3(xPos, yPos, 0f);

            t.tag = "MenuItems";

            return t;

    }

}
