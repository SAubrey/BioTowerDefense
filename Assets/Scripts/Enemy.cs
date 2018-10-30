using UnityEngine;
using UnityEngine.UI;
using System.Collections.Generic;

public class Enemy : MonoBehaviour {

    //[HideInInspector]
    public float speed;
    public float maxHealth;
    private float health;
    public string species; // Type of bacteria]
    
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

        //app.GetComponent<__increaseMutationChance(species, "vanc");

        setDestination();
        rollForMutate();
    }

    // Retrieved this functionality from https://www.raywenderlich.com/269-how-to-create-a-tower-defense-game-in-unity-part-1
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
		appScript.newScreenshake(6,0.1f);
        Destroy(gameObject);
    }

    public void hurt(int baseDamage, string antibioticType) {
        float effectiveness = appScript.antibiotics[antibioticType][species];
        float damage = baseDamage * effectiveness;

        if (resistances[antibioticType] == true) { // If resistant, null effect
            damage = 0;
        }
        health -= damage;
        updateHealthBar();

        if (health <= 0) {
            die(antibioticType);
        }
	}

    // TODO: Bacteria should only mutate against ab that can hurt it.
    public void rollForMutate() {
        //if ()
        // Roll for mutation against each antibacteria type.
        var line = appScript.mutationChances[species]["line"];
        if (Random.Range(0, 100) < line * 100) {
            resistances["amox"] = true;
            resistances["meth"] = true;
            resistances["vanc"] = true;
            resistances["carb"] = true;
            resistances["line"] = true;
            print(species + " has mutated against line! Likelihood: " + line);
            //GetComponent<SpriteRenderer>().color = Color.black;
            healthBar.color = Color.red;
            return;
        }
        var carb = appScript.mutationChances[species]["carb"];
        if (Random.Range(0, 100) < carb * 100) {
            resistances["amox"] = true;
            resistances["meth"] = true;
            resistances["vanc"] = true;
            resistances["carb"] = true;
            print(species + " has mutated against carb! Likelihood: " + carb);
			//GetComponent<SpriteRenderer>().color = Color.black;
			healthBar.color = (Color)(new Color32(255, 65, 0, 255));
            return;
        }
        var vanc = appScript.mutationChances[species]["vanc"];
        if (Random.Range(0, 100) < vanc * 100) {
            resistances["amox"] = true;
            resistances["meth"] = true;
            resistances["vanc"] = true;
            print(species + " has mutated against vanc! Likelihood: " + vanc);
			//GetComponent<SpriteRenderer>().color = Color.black;
			healthBar.color = (Color)(new Color32(155, 0, 250, 255));
            return;
        }
        var meth = appScript.mutationChances[species]["meth"];
         if (Random.Range(0, 100) < meth * 100) {
            resistances["amox"] = true;
            resistances["meth"] = true;
            print(species + " has mutated against meth! Likelihood: " + meth);
			//GetComponent<SpriteRenderer>().color = Color.black;
			healthBar.color = (Color)(new Color32(80, 80, 255, 255));
            return;
        }
        var amox = appScript.mutationChances[species]["amox"];
        if (Random.Range(0, 100) < amox * 100) {
            resistances["amox"] = true;
            print(species + " has mutated against amox! Likelihood: " + amox);
			//GetComponent<SpriteRenderer>().color = Color.black;
			healthBar.color = Color.green;
            return;
        }
       // More logic here 
    }

   private void die(string antibioticType) {
        // queue SFX
        level.GetComponent<EnemyManager>().incEnemiesDead();
        game.GetComponent<Game>().Currency += 2;
        appScript.increaseMutationChance(species, antibioticType);
		//Particles
		for(int i = 0;i<10;i++){
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

/* 
    public void setSpecies(Sprite img, string type) {
        species = type;
		switch(type){
			case "strep":
				transform.GetChild(2).GetComponent<SpriteRenderer>().color = Color.yellow;
				break;
			case "staph":
				transform.GetChild(2).GetComponent<SpriteRenderer>().color = Color.red;
				break;
			case "pneu":
				transform.GetChild(2).GetComponent<SpriteRenderer>().color = Color.cyan;
				break;
			case "TB":
				transform.GetChild(2).GetComponent<SpriteRenderer>().color = Color.magenta;
				break;
		}
        SpriteRenderer sr = transform.GetChild(2).GetComponent<SpriteRenderer>();
        sr.sprite = img;
    }
*/

    public void setSpecies(string type) {
        species = type;
        GetComponent<Animator>().Play(type);
    }
}