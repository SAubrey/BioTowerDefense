using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MoveEnemyTest : MonoBehaviour {

    [HideInInspector]
    public GameObject[] waypoints;
    private int currentWaypoint = 0;
    private float lastWaypointSwitchTime;
    public float speed;

    // Use this for initialization
    void Start () {
        lastWaypointSwitchTime = Time.time;
    }

    // Update is called once per frame
    // Retrieved this functionality from https://www.raywenderlich.com/269-how-to-create-a-tower-defense-game-in-unity-part-1
    void Update () {

        //Retrieve the position of the last waypoint the enemy crossed and the next waypoint
        Vector3 startPosition = waypoints[currentWaypoint].transform.position;
        Vector3 endPosition = waypoints[currentWaypoint + 1].transform.position;

        //Figure out length between waypoints and time to cover distance
        float pathLength = Vector3.Distance(startPosition, endPosition);
        float totalTimeForPath = pathLength / speed;

        //Figure out where it's currently at between the waypoints
        float currentTimeOnPath = Time.time - lastWaypointSwitchTime;

        //TODO: review Lerp functionality, it returns the linear interpolation between to points...what is linear interpolation
        gameObject.transform.position = Vector3.Lerp(startPosition, endPosition, currentTimeOnPath / totalTimeForPath);

        // Enemy has reached the next waypoint
        if (gameObject.transform.position.Equals(endPosition))
        {
            //If it's not the last waypoint "enemy has not made it to the base"
            if (currentWaypoint < waypoints.Length - 2)
            {

                currentWaypoint++;
                lastWaypointSwitchTime = Time.time;
                // TODO: Rotate into move direction
            }
            else
            {
				var audioObject = GameObject.Find("AudioObject");
				audioObject.GetComponent<AudioSource>().clip = Resources.Load("Sounds/hurt") as AudioClip;
				audioObject.GetComponent<AudioSource>().Play();
                Destroy(gameObject);
                // TODO: deduct health
            }
        }

    }
}
