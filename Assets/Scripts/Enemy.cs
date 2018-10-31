using UnityEngine;
using UnityEngine.UI;
using System.Collections.Generic;

public class Enemy : MonoBehaviour {

    //[HideInInspector]
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
        totalTimeForPath = pathLength / speed;
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
        if (baseDamage > 0) {
            if (resistances[antibioticType] == false) {
                rollForMutate(antibioticType);
            }
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

    // Mutation check happens at each projectile hit.
    public void rollForMutate(string antibioticType) {
        
        var chance = appScript.mutationChances[species][antibioticType];
        if (Random.Range(0, 100) < chance * 100) {
            print(species + " has mutated against " + antibioticType + "! Likelihood: " + (chance * 100) + "%");
            setResistance(antibioticType);
        }
    }

    // Recursive calls handle hierarchy of antibiotics.
    private void setResistance(string antibioticType) {
        switch(antibioticType) {
            case "amox":
                setResistances(new string[] {"amox"});
                healthBar.color = Color.green;
                break;
            case "meth":
                setResistances(new string[] {"amox", "meth"});
                healthBar.color = (Color)(new Color32(80, 80, 255, 255));
                break;
            case "vanc":
                setResistances(new string[] {"amox", "meth", "vanc"});
                healthBar.color = (Color)(new Color32(155, 0, 250, 255));
                break;
            case "carb":
                setResistances(new string[] {"amox", "meth", "vanc", "carb"});
                healthBar.color = (Color)(new Color32(255, 65, 0, 255));
                break;
            case "line":
                setResistances(new string[] {"amox", "meth", "vanc", "carb", "line"});
                healthBar.color = Color.red;
                break;
            case "rifa":
                resistances["rifa"] = true;
                break;
            case "ison":
                resistances["ison"] = true;
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
		for(int i = 0; i < 10; i++){
			GameObject particle = Resources.Load("Prefabs/Particle") as GameObject;
			particle.transform.position = transform.position;
			particle.GetComponent<SpriteRenderer>().color = transform.GetChild(2).GetComponent<SpriteRenderer>().color;
			Instantiate(particle);
		}
        Destroy(gameObject);
    }

    private void updateHealthBar() {
        healthBar.fillAmount = health / maxHealth;
    }

    public void setSpecies(string type) {
        species = type;
        GetComponent<Animator>().Play(type);
    }
}