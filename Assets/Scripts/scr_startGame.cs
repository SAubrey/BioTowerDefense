using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class scr_startGame : MonoBehaviour {

    public GameObject[] waypoints;

	public void StartGame(GameObject Enemy){
        //Instantiate(Enemy, new Vector3(-10.5f,3.25f,0f),Quaternion.identity);
        Instantiate(Enemy);
        Enemy.GetComponent<MoveEnemyTest>().waypoints = waypoints;

    }
}
