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
			transform.position += new Vector3(0f,(0f - transform.position.y)*0.3f,0f);
		}
		else{
			transform.position += new Vector3(0f,(-12f - transform.position.y)*0.1f,0f);
		}
		
		if(transform.position.y < -10 && !Game.paused){
			gameObject.SetActive(false);
		}
	}
}
