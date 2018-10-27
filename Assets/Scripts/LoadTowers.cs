using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
using UnityEngine.UI;

public class LoadTowers : MonoBehaviour {
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
                                {"antibioticType", "amox"},
                                {"towerType", 0},
                                {"targetType", 0}, 
                                {"cost", 10},
                                {"radius", 4f},
                                {"cooldown", 30.0f},
                                {"towerSprite", amoxSprite},
                                {"towerColor", Color.gray},
                                {"projectileSprite", projectile},
                                };

    public static IDictionary<string, object> methTower = new Dictionary<string, object>(){
                                {"name", "Methicillin"},
                                {"towerPositionX", 8f},
                                {"towerPositionY", 2f},
                                {"antibioticType", "meth"},
                                {"towerType", 0},
                                {"targetType", 0}, 
                                {"cost", 15},
                                {"radius", 4f},
                                {"cooldown", 30.0f},
                                {"towerSprite", methSprite},
                                {"towerColor", Color.magenta},
                                {"projectileSprite", projectile} };

    public static IDictionary<string, object> vancTower = new Dictionary<string, object>(){
                                {"name", "Vancocymin"},
                                {"towerPositionX", 8f},
                                {"towerPositionY", 1f},
                                {"antibioticType", "vanc"},
                                {"towerType", 0},
                                {"targetType", 0},
                                {"cost", 20},
                                {"radius", 4f},
                                {"cooldown", 30.0f},
                                {"towerSprite", vancSprite},
                                {"towerColor", Color.blue},
                                {"projectileSprite", projectile},
                                 };

    public static IDictionary<string, object> carbTower = new Dictionary<string, object>(){
                                {"name", "Carbapenem"},
                                {"towerPositionX", 8f},
                                {"towerPositionY", 0f},
                                {"antibioticType", "carb"},
                                {"towerType", 0},
                                {"targetType", 0}, 
                                {"cost", 30},
                                {"radius", 4f},
                                {"cooldown", 30.0f},
                                {"towerSprite", carbSprite},
                                {"towerColor", Color.yellow},
                                {"projectileSprite", projectile},
                                };

    public static IDictionary<string, object> lineTower = new Dictionary<string, object>(){
                                {"name", "Linezolid"},
                                {"towerPositionX", 8f},
                                {"towerPositionY", -1f},
                                {"antibioticType", "line"},
                                {"towerType", 0},
                                {"targetType", 0}, 
                                {"cost", 40},
                                {"radius", 4f},
                                {"cooldown", 30.0f},
                                {"towerSprite", lineSprite},
                                {"towerColor", Color.green},
                                {"projectileSprite", projectile},
                                };

    public static IDictionary<string, object> rifaTower = new Dictionary<string, object>(){
                                {"name", "Rifampicin"},
                                {"towerPositionX", 8f},
                                {"towerPositionY", -2f},
                                {"antibioticType", "rifa"},
                                {"towerType", 0},
                                {"targetType", 0}, 
                                {"cost", 30},
                                {"radius", 4f},
                                {"cooldown", 30.0f},
                                {"towerSprite", rifaSprite},
                                {"towerColor", Color.red},
                                {"projectileSprite", projectile},
                                 };

    public static IDictionary<string, object> isonTower = new Dictionary<string, object>(){
                                {"name", "Isoniazid"},
                                {"towerPositionX", 8f},
                                {"towerPositionY", -3f},
                                {"antibioticType", "ison"},
                                {"towerType", 0},
                                {"targetType", 0}, 
                                {"cost", 30},
                                {"radius", 4f},
                                {"cooldown", 30.0f},
                                {"towerSprite", isonSprite},
                                {"towerColor", Color.cyan},
                                {"projectileSprite", projectile},
                                 };

    public IDictionary<string, IDictionary<string, object>> towers = new Dictionary<string, IDictionary<string, object>>(){
                                    {"Amoxicillin", amoxTower},
                                    {"Methicillin", methTower},
                                    {"Vancocymin", vancTower},
                                   {"Carbapenem", carbTower},
                                   {"Linezolid", lineTower},
                                   {"Rifampicin", rifaTower},
                                   {"Isoniazid", isonTower} };

/* 
    private static IDictionary<string, object> pellet = new Dictionary<string, object>() {
                                {"additionalCost", 0},
                                {"radius", 4f},
                                {"cooldown", 30.0f}, };
                                */

 // USE ADDITIONAL DICTIONARIES BASED ON TOWER TYPE TO ADJUST VALUES BEING SET
    private GameObject tower;

    private __app appTowers;

    // Use this for initialization
    void Start() {
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
    void Update() {
    }

    //Reloads towers, called by the DragAndDrop script attached to individual towers
    public void reloadTower(string towerName) {
      
        IDictionary <string, object> towerAttributes;
        towerAttributes = towers[towerName];

        GameObject newTower = setTowerAttributes(towerAttributes);
        Instantiate(newTower, gameObject.transform, true);
    }

    //Loads all towers on start
    void LoadAllTowers() {
        foreach (KeyValuePair<string, IDictionary<string, object>> tow in towers) {     
            GameObject newTower = setTowerAttributes(tow.Value);
            Instantiate(newTower, gameObject.transform, true);
        }
    }

    //Set the towers attributes before spawning to the menu
    GameObject setTowerAttributes(IDictionary<string, object> attributes) {

        GameObject t = tower;
        Tower tScript = tower.GetComponent<Tower>();

        tScript.towerName = (string) attributes["name"];
        tScript.antibioticType = (string) attributes["antibioticType"];
        tScript.type = (int) attributes["towerType"];
        tScript.targetType = (int) attributes["targetType"]; 
        tScript.cost = (int) attributes["cost"];
        tScript.detectionRadius = (float)attributes["radius"];
        tScript.coolDown = (float)attributes["cooldown"];
        //tScript.specialEffect = (int) attributes["specialEffect"];
        t.GetComponent<SpriteRenderer>().sprite = (Sprite) attributes["towerSprite"];
        t.GetComponent<SpriteRenderer>().color = (Color) attributes["towerColor"];
        tScript.projectileSprite = null;

        float xPos =  (float) attributes["towerPositionX"];
        float yPos =  (float) attributes["towerPositionY"];

        tScript.transform.position = new Vector3(xPos, yPos, 0f);

        tScript.tag = "MenuItems";

        return t;
    }
}