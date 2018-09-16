using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Game : MonoBehaviour {
	public static bool game = false;
	public GameObject[] waypoints;
	private AudioClip startSound;
	private AudioSource audio;
	private int time;
	void Start () {
		
	}

	// Use this for initialization
	public void StartGame () {
		game = true;
	}
	
	// Update is called once per frame
	void Update () {
		if(game){
			if(time >= 60){
				GameObject enemy  = Resources.Load("Enemy") as GameObject;
				SpawnEnemy (enemy);
				time = 0;
			}
			time++;
		}
	}

	public void SpawnEnemy(GameObject Enemy){
        //Instantiate(Enemy, new Vector3(-10.5f,3.25f,0f),Quaternion.identity);
        GameObject newEnemy = Instantiate(Enemy);
        newEnemy.GetComponent<MoveEnemyTest>().waypoints = waypoints;
    }
}
