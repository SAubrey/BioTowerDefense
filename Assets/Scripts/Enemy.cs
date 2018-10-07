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
    private Vector3 startPosition;
    private Vector3 endPosition;
    private float currentTimeOnPath;
    private float totalTimeForPath;
    private GameObject audioObject;

    [Header("Unity specific")]
    public Image healthBar;
    public GameObject[] waypoints;


    // Dictionaries are arranged in order of effectiveness up to carb (1-5)
    // Linezolid is its own category, rifampicin and isoniazid are their own category.
    private static IDictionary<string, float> amox = new Dictionary<string, float>() {
                                        {"staph", 1f},
                                        {"strep", 1f},
                                        {"pneu", .4f},
                                        {"TB", 0f} };
    private static IDictionary<string, float> meth = new Dictionary<string, float>() {
                                        {"staph", 1f},
                                        {"strep", 1f},
                                        {"pneu", .4f},
                                        {"TB", 0f} };
    private static IDictionary<string, float> vanc = new Dictionary<string, float>() {
                                        {"staph", 1f},
                                        {"strep", 1f},
                                        {"pneu", .4f},
                                        {"TB", 0f} };
    private static IDictionary<string, float> carb = new Dictionary<string, float>() {
                                        {"staph", 1f},
                                        {"strep", 1f},
                                        {"pneu", 1f}, // best. If resistant, 1, 2, 3 useless.
                                        {"TB", 0f} };
    private static IDictionary<string, float> line = new Dictionary<string, float>() {
                                        {"staph", 1f},
                                        {"strep", 1f},
                                        {"pneu", .9f}, // second best
                                        {"TB", 0f} };
    private static IDictionary<string, float> rifa = new Dictionary<string, float>() {
                                        {"staph", .1f},
                                        {"strep", .1f},
                                        {"pneu", .1f},
                                        {"TB", .5f} };
    private static IDictionary<string, float> ison = new Dictionary<string, float>() {
                                        {"staph", .1f},
                                        {"strep", .1f},
                                        {"pneu", .1f},
                                        {"TB", .5f} };                                                                                
   
    private static IDictionary<string, IDictionary<string, float>> antibiotics = new Dictionary<string, IDictionary<string, float>>() {
                                        {"amox", amox},
                                        {"meth", meth},
                                        {"vanc", vanc},
                                        {"carb", carb},
                                        {"line", line},
                                        {"rifa", rifa},
                                        {"ison", ison} };

    // Use this for initialization
    void Start () {
        health = maxHealth;
        lastWaypointSwitchTime = Time.time;
        game = GameObject.Find("Game");
        audioObject = GameObject.Find("AudioObject");
        setDestination();
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
        audioObject.GetComponent<AudioSource>().clip = Resources.Load("Sounds/hurt") as AudioClip;
        audioObject.GetComponent<AudioSource>().Play();
        game.GetComponent<Game>().takeDamage(1);
        game.GetComponent<EnemyManager>().incEnemiesDead();
        Destroy(gameObject);
    }

    public void hurt(int baseDamage, string antibioticType) {
        float effectiveness = antibiotics[antibioticType][species];
        float damage = baseDamage * effectiveness;
        health -= baseDamage;
        updateHealthBar();

        if (health <= 0) {
            die();
        }
	}

    private void die() {
        // queue SFX
        game.GetComponent<EnemyManager>().incEnemiesDead();
        game.GetComponent<Game>().Currency += 2;
        Destroy(gameObject);
    }

    private void updateHealthBar() {
        healthBar.fillAmount = health / maxHealth;
    }
}
