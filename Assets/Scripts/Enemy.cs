using UnityEngine;
using UnityEngine.UI;
using System.Collections.Generic;

public class Enemy : MonoBehaviour {

    //[HideInInspector]
	public float speedActual;
    public float speed;
    public float maxHealth;
    private float health;
    public string species; // Type of bacteria
    
    private int currentWaypoint = 0;
    private float lastWaypointSwitchTime;
    private GameObject game;
	private GameObject level;
    private __app appScript;
    private Vector3 startPosition, endPosition;
    private float currentTimeOnPath, totalTimeForPath;
    private GameObject audioObject;

    [Header("Unity specific")]
    public Image healthBar;
    public GameObject[] waypoints;

    private IDictionary<string, bool> resistances = new Dictionary<string, bool>() {
                                        {"amox", false},
                                        {"meth", false},
                                        {"vanc", false},
                                        {"carb", false},
                                        {"line", false},
                                        {"rifa", false},
                                        {"ison", false} };

    // Use this for initialization
    void Start () {
		speedActual = speed;
        health = maxHealth;
        lastWaypointSwitchTime = Time.time;
        game = GameObject.Find("Game");
		level = GameObject.FindGameObjectsWithTag("Level")[0];
        appScript = GameObject.Find("__app").GetComponent<__app>();
        audioObject = GameObject.Find("AudioObject");

        setDestination();
    }

    void Update () {
		if (Game.paused || !Game.game) {
			return;
		}
        //bool reachedDestination = checkReachedDestination();
		
        if (checkReachedDestination()) {
            setDestination();
        }
        move();
		speedActual = speed*game.GetComponent<Game>().timescale;
		Debug.Log("Speed: "+(speed)+", timescale: "+game.GetComponent<Game>().timescale+", speedActual: "+speedActual);
    }
    private void move() {
        // Figure out where it's currently at between the waypoints
        currentTimeOnPath = Time.time - lastWaypointSwitchTime;

        // The linear interpolant is the line between two points. Lerp returns the position upon that line.
        gameObject.transform.position = Vector3.Lerp(startPosition, endPosition, currentTimeOnPath / totalTimeForPath);
    }

    private bool checkReachedDestination() {
          // Enemy has reached the next waypoint
        if (gameObject.transform.position.Equals(endPosition)) {
            //If it's not the last waypoint "enemy has not made it to the base"
            if (currentWaypoint < waypoints.Length - 2) {
                currentWaypoint++;
                lastWaypointSwitchTime = Time.time;
                // TODO: Rotate into move direction
            } else {
				reachOrgan();
                return false;
            }
            return true;
        } else {
            return false;
        }
    }

    // Only needs updating when a waypoint has been reached.
    private void setDestination() {
         // Retrieve the position of the last waypoint the enemy crossed and the next waypoint
        startPosition = waypoints[currentWaypoint].transform.position;
        endPosition = waypoints[currentWaypoint + 1].transform.position;

        // Figure out length between waypoints and time to cover distance
        float pathLength = Vector3.Distance(startPosition, endPosition);
        totalTimeForPath = pathLength / speedActual;
    }

    private void reachOrgan() {
        //audioObject.GetComponent<AudioSource>().clip = Resources.Load("Sounds/hurt") as AudioClip;
       // audioObject.GetComponent<AudioSource>().Play();
        game.GetComponent<Game>().takeDamage(1);
        level.GetComponent<EnemyManager>().incEnemiesDead();
		appScript.newScreenshake(6, 0.1f);
        Destroy(gameObject);
    }

    public void hurt(int baseDamage, string antibioticType) {
        if (baseDamage > 0) { // So that TB cannot mutate to 1-5 abs.
            // So that pneu, staph, strep cannot mutate to rifa and ison.
            if (resistances[antibioticType] == false && 
               !(species != "TB" && (antibioticType == "rifa" || antibioticType == "ison"))) { 
                rollForMutate(antibioticType);
            }
        

            if (resistances[antibioticType] == false) { // If not resistant, do damage
                float effectiveness = appScript.antibiotics[antibioticType][species];
                health -= baseDamage * effectiveness;
                updateHealthBar();

                if (health <= 0) {
                    die();
                }
            }   
        }
	}

    // Mutation check happens at each projectile hit.
    public void rollForMutate(string antibioticType) {
        
        var chance = appScript.mutationChances[species][antibioticType];
        if (Random.Range(0, 100) < chance * 100) {
            print(species + " has mutated against " + antibioticType + "! Likelihood: " + (chance * 100) + "%");
            setResistance(antibioticType);
			appScript.newParticles(transform.position,30,0.8f,Color.black);
        }
    }

    private void setResistance(string antibioticType) {
        switch(antibioticType) {
            case "amox":
                setResistances(new string[] {"amox"});
                healthBar.color = LoadTowers.amoxColor;
                break;
            case "meth":
                setResistances(new string[] {"amox", "meth"});
                healthBar.color = LoadTowers.methColor;
                break;
            case "vanc":
                setResistances(new string[] {"amox", "meth", "vanc"});
                healthBar.color = LoadTowers.vancColor;
                break;
            case "carb":
                setResistances(new string[] {"amox", "meth", "vanc", "carb"});
                healthBar.color = LoadTowers.carbColor;
                break;
            case "line":
                setResistances(new string[] {"amox", "meth", "vanc", "carb", "line"});
                healthBar.color = LoadTowers.lineColor;
                break;
            case "rifa":
                resistances["rifa"] = true;
                healthBar.color = LoadTowers.rifaColor;
                break;
            case "ison":
                resistances["ison"] = true;
                healthBar.color = LoadTowers.isonColor;
                break;
        }
    }

    private void setResistances(string[] abs) {
        foreach (string ab in abs) {
            resistances[ab] = true;
        }
    }

    private void die() {
        // queue SFX
        level.GetComponent<EnemyManager>().incEnemiesDead();
        game.GetComponent<Game>().Currency += 2;
        
		//Particles
		appScript.newParticles(transform.position,10,0.05f,transform.GetChild(2).gameObject.GetComponent<SpriteRenderer>().color);
        Destroy(gameObject);
    }

    private void updateHealthBar() {
        healthBar.fillAmount = health / maxHealth;
    }

    public void setSpecies(string type) {
        species = type;
        GetComponent<Animator>().Play(type);
		//Set 'Color'
		switch(type){
			case("strep"):
				transform.GetChild(2).gameObject.GetComponent<SpriteRenderer>().color = Color.blue;
				break;
			case("staph"):
				transform.GetChild(2).gameObject.GetComponent<SpriteRenderer>().color = Color.blue;
				break;
			case("pneu"):
				transform.GetChild(2).gameObject.GetComponent<SpriteRenderer>().color = Color.magenta;
				break;
			case("TB"):
				transform.GetChild(2).gameObject.GetComponent<SpriteRenderer>().color = Color.magenta;
				break;
		}
		
    }
}