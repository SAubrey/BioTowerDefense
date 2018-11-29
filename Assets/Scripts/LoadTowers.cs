using System;
using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
using UnityEngine.UI;

public class LoadTowers : MonoBehaviour {
        public Sprite pelletTower;
        public Sprite laserTower;
        public Sprite bombTower;

        public static GameObject projectile;

        private int showTowers;

        public Button navUp;
        public Button navDown;

        private bool scrolling;
        private float destination;
        private IDictionary<int, Vector2> scrollCoordinates;

        // TYPE
        private static int projType = 0;
        private static int laserType = 1;
        private static int bombType = 2;
        

        private static float posX = 8.25f;
        private static Vector2 ppos = new Vector2(8.25f, 3.65f);
        private static Vector2 lpos = new Vector2(8.25f, 11.2f);
        private static Vector2 bpos = new Vector2(posX, 18.75f);
        private static float yDist = 1f;
        
        // RADIUS
        public static float baseRadius = 3f;
        public static float extraBombRadius = -.5f;
        public static float extraLaserRadius = .8f;

        // COOLDOWN
        public static float baseCooldown = 0.5f; // seconds
        public static float extraBombCooldown = 1f;
        public static float extraLaserCooldown = 1f;

        // COST
        public static int baseCost = 20;
        public static int extraBombCost = 20;
        public static int extraLaserCost = 20;
        public static int amoxCost = baseCost;
        public static int methCost = baseCost;
        public static int vancCost = baseCost;
        public static int carbCost = baseCost;
        public static int lineCost = baseCost;
        public static int rifaCost = baseCost;
        public static int isonCost = baseCost;

    public static IDictionary<string, object> amoxProjectile = new Dictionary<string, object>(){
                {"name", "Amoxicillin"},
                {"antibioticType", "amox"},
                {"position", new Vector2(ppos.x, ppos.y - yDist * 1.2f)},
                {"cost", amoxCost},
                {"type", projType},
                {"radius", baseRadius},
                {"cooldown", baseCooldown},
                {"towerColor", __app.amoxColor} };

    public static IDictionary<string, object> methProjectile = new Dictionary<string, object>(){
                {"name", "Methicillin"},
                {"antibioticType", "meth"},
                {"position", new Vector2(ppos.x, ppos.y - yDist * 2.1f)},
                {"cost", methCost},
                {"type", projType},
                {"radius", baseRadius},
                {"cooldown", baseCooldown},
                {"towerColor",  __app.methColor} };

    public static IDictionary<string, object> vancProjectile = new Dictionary<string, object>(){
                {"name", "Vancomycin"},
                {"antibioticType", "vanc"},
                {"position", new Vector2(ppos.x, ppos.y - yDist * 3.1f)},
                {"cost", vancCost},
                {"type", projType},
                {"radius", baseRadius},
                {"cooldown", baseCooldown},
                {"towerColor",  __app.vancColor} };

    public static IDictionary<string, object> carbProjectile = new Dictionary<string, object>(){
                {"name", "Carbapenem"},
                {"antibioticType", "carb"},
                {"position", new Vector2(ppos.x, ppos.y - yDist * 4)},
                {"cost", carbCost},
                {"type", projType},
                {"radius", baseRadius},
                {"cooldown", baseCooldown},
                {"towerColor",  __app.carbColor} };

    public static IDictionary<string, object> lineProjectile = new Dictionary<string, object>(){
                {"name", "Linezolid"},
                {"antibioticType", "line"},
                {"position", new Vector2(ppos.x, ppos.y - yDist * 5)},
                {"cost", lineCost},
                {"type", projType},
                {"radius", baseRadius},
                {"cooldown", baseCooldown},
                {"towerColor",  __app.lineColor} };

    public static IDictionary<string, object> rifaProjectile = new Dictionary<string, object>(){
                {"name", "Rifampicin"},
                {"antibioticType", "rifa"},
                {"position", new Vector2(ppos.x, ppos.y - yDist * 6)},
                {"cost", rifaCost},
                {"type", projType},
                {"radius", baseRadius},
                {"cooldown", baseCooldown},
                {"towerColor",  __app.rifaColor} };

    public static IDictionary<string, object> isonProjectile = new Dictionary<string, object>(){
                {"name", "Isoniazid"},
                {"antibioticType", "ison"},
                {"position", new Vector2(ppos.x, ppos.y - yDist * 7)},
                {"cost", isonCost},
                {"type", projType},
                {"radius", baseRadius},
                {"cooldown", baseCooldown},
                {"towerColor",  __app.isonColor} };

public static IDictionary<string, object> amoxHitscan = new Dictionary<string, object>(){
                {"name", "Amoxicillin Laser"},
                {"antibioticType", "amox"},
                {"position", new Vector2(lpos.x, lpos.y - yDist * 1)},
                {"cost", amoxCost + extraLaserCost},
                {"type", laserType},
                {"radius", baseRadius + extraLaserRadius},
                {"cooldown", baseCooldown + extraLaserCooldown},
                {"towerColor",  __app.amoxColor} };

public static IDictionary<string, object> methHitscan = new Dictionary<string, object>(){
                {"name", "Methicillin Laser"},
                {"antibioticType", "meth"},
                {"position", new Vector2(lpos.x, lpos.y - yDist * 2)},
                {"cost", methCost + extraLaserCost},
                {"type", laserType},
                {"radius", baseRadius + extraLaserRadius},
                {"cooldown", baseCooldown + extraLaserCooldown},
                {"towerColor", __app.methColor} };

 public static IDictionary<string, object> vancHitscan = new Dictionary<string, object>(){
                {"name", "Vancomycin Laser"},
                {"antibioticType", "vanc"},
                {"position", new Vector2(lpos.x, lpos.y - yDist * 3)},
                {"cost", vancCost + extraLaserCost},
                {"type", laserType},
                {"radius", baseRadius + extraLaserRadius},
                {"cooldown", baseCooldown + extraLaserCooldown},
                {"towerColor", __app.vancColor} };

 public static IDictionary<string, object> carbHitscan = new Dictionary<string, object>(){
                {"name", "Carbapenem Laser"},
                {"antibioticType", "carb"},
                {"position", new Vector2(lpos.x, lpos.y - yDist * 4)},
                {"cost", carbCost + extraLaserCost},
                {"type", laserType},
                {"radius", baseRadius + extraLaserRadius},
                {"cooldown", baseCooldown + extraLaserCooldown},
                {"towerColor", __app.carbColor} };

 public static IDictionary<string, object> lineHitscan = new Dictionary<string, object>(){
                {"name", "Linezolid Laser"},
                {"antibioticType", "line"},
                {"position", new Vector2(lpos.x, lpos.y - yDist * 5)},
                {"cost", lineCost + extraLaserCost},
                {"type", laserType},
                {"radius", baseRadius + extraLaserRadius},
                {"cooldown", baseCooldown + extraLaserCooldown},
                {"towerColor", __app.lineColor} };

    public static IDictionary<string, object> rifaHitscan = new Dictionary<string, object>(){
                {"name", "Rifampicin Laser"},
                {"antibioticType", "rifa"},
                {"position", new Vector2(lpos.x, lpos.y - yDist * 6)},
                {"cost", rifaCost + extraLaserCost},
                {"type", laserType},
                {"radius", baseRadius + extraLaserRadius},
                {"cooldown", baseCooldown + extraLaserCooldown},
                {"towerColor", __app.rifaColor} };

    public static IDictionary<string, object> isonHitscan = new Dictionary<string, object>(){
                {"name", "Isoniazid Laser"},
                {"antibioticType", "ison"},
                {"position", new Vector2(lpos.x, lpos.y - yDist * 7)},
                {"cost", isonCost + extraLaserCost},
                {"type", laserType},
                {"radius", baseRadius + extraLaserRadius},
                {"cooldown", baseCooldown + extraLaserCooldown},
                {"towerColor", __app.isonColor} };

public static IDictionary<string, object> amoxAOE = new Dictionary<string, object>(){
                {"name", "Amoxicillin Bomber"},
                {"antibioticType", "amox"},
                {"position", new Vector2(bpos.x, bpos.y - yDist * 1.1f)},
                {"cost", amoxCost + extraBombCost},
                {"type", bombType},
                {"radius", baseRadius + extraBombRadius},
                {"cooldown", baseCooldown + extraBombCooldown},
                {"towerColor", __app.amoxColor} };

public static IDictionary<string, object> methAOE = new Dictionary<string, object>(){
                {"name", "Methicillin Bomber"},
                {"antibioticType", "meth"},
                {"position", new Vector2(bpos.x, bpos.y - yDist * 2.1f)},
                {"cost", methCost + extraBombCost},
                {"type", bombType},
                {"radius", baseRadius + extraBombRadius},
                {"cooldown", baseCooldown + extraBombCooldown},
                {"towerColor", __app.methColor} };

public static IDictionary<string, object> vancAOE = new Dictionary<string, object>(){
                {"name", "Vancomycin Bomber"},
                {"antibioticType", "vanc"},
                {"position", new Vector2(bpos.x, bpos.y - yDist * 3)},
                {"cost", vancCost + extraBombCost},
                {"type", bombType},
                {"radius", baseRadius + extraBombRadius},
                {"cooldown", baseCooldown + extraBombCooldown},
                {"towerColor", __app.vancColor} };

public static IDictionary<string, object> carbAOE = new Dictionary<string, object>(){
                {"name", "Carbapenem Bomber"},
                {"antibioticType", "carb"},
                {"position", new Vector2(bpos.x, bpos.y - yDist * 3.95f)},
                {"cost", carbCost + extraBombCost},
                {"type", bombType},
                {"radius", baseRadius + extraBombRadius},
                {"cooldown", baseCooldown + extraBombCooldown},
                {"towerColor", __app.carbColor} };

public static IDictionary<string, object> lineAOE = new Dictionary<string, object>(){
                {"name", "Linezolid Bomber"},
                {"antibioticType", "line"},
                {"position", new Vector2(bpos.x, bpos.y - yDist * 4.9f)},
                {"cost", lineCost + extraBombCost},
                {"type", bombType},
                {"radius", baseRadius + extraBombRadius},
                {"cooldown", baseCooldown + extraBombCooldown},
                {"towerColor", __app.lineColor} };

public static IDictionary<string, object> rifaAOE = new Dictionary<string, object>(){
                {"name", "Rifampicin Bomber"},
                {"antibioticType", "rifa"},
                {"position", new Vector2(bpos.x, bpos.y - yDist * 5.9f)},
                {"cost", rifaCost + extraBombCost},
                {"type", bombType},
                {"radius", baseRadius + extraBombRadius},
                {"cooldown", baseCooldown + extraBombCooldown},
                {"towerColor", __app.rifaColor} };

public static IDictionary<string, object> isonAOE = new Dictionary<string, object>(){
                {"name", "Isoniazid Bomber"},
                {"antibioticType", "ison"},
                {"position", new Vector2(bpos.x, bpos.y - yDist * 6.9f)},
                {"cost", isonCost + extraBombCost},
                {"type", bombType},
                {"radius", baseRadius + extraBombRadius},
                {"cooldown", baseCooldown + extraBombCooldown},
                {"towerColor", __app.isonColor} };

    public IDictionary<string, IDictionary<string, object>> towers = new Dictionary<string, IDictionary<string, object>>(){
                {"Amoxicillin", amoxProjectile},
                {"Methicillin", methProjectile},
                {"Vancomycin", vancProjectile},
                {"Carbapenem", carbProjectile},
                {"Linezolid", lineProjectile},
                {"Rifampicin", rifaProjectile},
                {"Isoniazid", isonProjectile},
                {"Amoxicillin Laser", amoxHitscan},
                {"Methicillin Laser", methHitscan},
                {"Vancomycin Laser", vancHitscan},
                {"Carbapenem Laser", carbHitscan},
                {"Linezolid Laser", lineHitscan},
                {"Rifampicin Laser", rifaHitscan},
                {"Isoniazid Laser", isonHitscan},
                {"Amoxicillin Bomber", amoxAOE},
                {"Methicillin Bomber", methAOE},
                {"Vancomycin Bomber", vancAOE},
                {"Carbapenem Bomber", carbAOE},
                {"Linezolid Bomber", lineAOE},
                {"Rifampicin Bomber", rifaAOE},
                {"Isoniazid Bomber", isonAOE} };

    private GameObject tower;

    void Start() {
        tower = Resources.Load("Prefabs/Tower") as GameObject;
        projectile = Resources.Load("Prefabs/Projectile") as GameObject;

        showTowers = 0;
        destination = 6;
        scrolling = false;
        LoadAllTowers();

        scrollCoordinates =  new Dictionary<int, Vector2>() {
							{0, new Vector2(0f, 7.0f)},
							{1, new Vector2(0f, -0.5f)},
							{2, new Vector2(0f, -8.1f)} };
    }

    void Update() {
        if (scrolling) {
            scrollTowers(destination);        
        }
    }

    // Creates a new tower, called by the DragAndDrop script attached to individual towers
    public void reloadTower(string towerName, Vector3 relativePosition) {
        IDictionary <string, object> towerAttributes;
        towerAttributes = towers[towerName];
        GameObject newTower = setTowerAttributes(towerAttributes);

        newTower.transform.position = relativePosition;
    }

    //Loads the first column of towers on start, will load the displayed column
    private void LoadAllTowers() {
        foreach (KeyValuePair<string, IDictionary<string, object>> tow in towers) {    
            setTowerAttributes(tow.Value);
        }
    }

    // Set the towers attributes before spawning to the menu
    private GameObject setTowerAttributes(IDictionary<string, object> attributes) {
        Vector2 p = (Vector2) attributes["position"];

        GameObject t = Instantiate(tower, gameObject.transform, true);

        Tower tScript = t.GetComponent<Tower>();
        SpriteRenderer sr = t.GetComponent<SpriteRenderer>();

        tScript.type = (int) attributes["type"];
        if (tScript.type == projType) {
            sr.sprite = pelletTower;
        } 
        else if (tScript.type == laserType) {
            sr.sprite = laserTower;
        }
        else if (tScript.type == bombType) {
            sr.sprite = bombTower;
        }

        tScript.towerName = (string) attributes["name"];
        tScript.antibioticType = (string) attributes["antibioticType"];
        tScript.cost = (int) attributes["cost"];
        tScript.detectionRadius = (float)attributes["radius"];
        tScript.coolDown = (float)attributes["cooldown"];
        sr.color = (Color) attributes["towerColor"];
        t.transform.position = new Vector3(p.x, p.y, -2f);
        tScript.tag = "MenuItems"; 

        tScript.manualStart();
        return t;
    }

    public void ShowTowerColumn(int step) {
        showTowers += step;
        scrolling = true;

        if (showTowers == 2) {
             navUp.interactable = false;
        } else {
             navUp.interactable = true;
        }
        if (showTowers == 0) {
              navDown.interactable = false;
        } else {
            navDown.interactable = true;
        }
    }

    void scrollTowers(float y) {
        Vector3 position = transform.position;
        Vector2 dest = scrollCoordinates[showTowers];

		float dy = dest.y - position.y;
		position.y += dy * 0.1f;
		transform.position = position;

        if (Math.Abs(dy) < .02) {
			scrolling = false;
		}
    }
}