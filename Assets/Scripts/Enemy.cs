using UnityEngine;
using UnityEngine.UI;
using System.Collections.Generic;

public class Enemy : MonoBehaviour {

    //[HideInInspector]
    public float speed;
    public float maxHealth;
    private float health;
    public string species;
    private int currentWaypoint = 0;
    private float lastWaypointSwitchTime;
    private GameObject game;
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
        //appScript = GameObject.Find("__app").GetComponent<__app>();
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
        game.GetComponent<EnemyManager>().incEnemiesDead();
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

    public void rollForMutate() {
        // Roll for mutation against each antibacteria type.
        var carb = appScript.mutationChances[species]["carb"];
        if (Random.Range(0, 100) <= carb * 100) {
            resistances["amox"] = true;
            resistances["meth"] = true;
            resistances["vanc"] = true;
            resistances["carb"] = true;
            print(species + " has mutated against carb!");
            return;
        }
    
        var vanc = appScript.mutationChances[species]["vanc"];
        if (Random.Range(0, 100) <= vanc * 100) {
            resistances["amox"] = true;
            resistances["meth"] = true;
            resistances["vanc"] = true;
            print(species + " has mutated against vanc!");
            return;
        }
        var meth = appScript.mutationChances[species]["meth"];
         if (Random.Range(0, 100) <= meth * 100) {
            resistances["amox"] = true;
            resistances["meth"] = true;
            print(species + " has mutated against meth!");
            return;
        }
        var amox = appScript.mutationChances[species]["amox"];
        if (Random.Range(0, 100) <= amox * 100) {
            resistances["amox"] = true;
            print(species + " has mutated against amox!");
            return;
        }

       // More logic here 
    }

    private void die(string antibioticType) {
        // queue SFX
        game.GetComponent<EnemyManager>().incEnemiesDead();
        game.GetComponent<Game>().Currency += 2;
        appScript.increaseMutationChance(species, antibioticType);
        Destroy(gameObject);
    }

    private void updateHealthBar() {
        healthBar.fillAmount = health / maxHealth;
    }
}
