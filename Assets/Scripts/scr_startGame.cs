using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class scr_startGame : MonoBehaviour {
	public void StartGame(GameObject Enemy){
			Instantiate(Enemy, new Vector3(-9f,0f,0f),Quaternion.identity);
	}
}
