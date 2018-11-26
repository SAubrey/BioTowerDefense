using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class pauseMenu : MonoBehaviour {
	private Vector3 dest;
	private GameObject game;
	private Game gameScript;
	// Use this for initialization
	void Start () {

	}
	
	// Update is called once per frame
	void Update () {
		if(Game.paused){
			dest = new Vector3(0f,1f,0f);
		}
		else{
			dest = new Vector3(0f,0f,0f);
		}
		float myDiffX;
		float myDiffY;
		myDiffX = dest.y - transform.position.y * 0.1f;
		myDiffY = dest.x - transform.position.x * 0.1f;
		transform.Translate(new Vector3(myDiffX,myDiffY,0f),Space.World);
	}
}
