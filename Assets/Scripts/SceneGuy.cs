using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class SceneGuy : MonoBehaviour {
	public Scene currentScene;
	public Scene previousScene;
	// Use this for initialization
	void Start () {
		currentScene = SceneManager.GetActiveScene ();
		previousScene = SceneManager.GetActiveScene ();
	}
	
	// Update is called once per frame
	void Update () {
		//Scene Management
		/*
		if(SceneManager.GetActiveScene != currentScene){
			previousScene = currentScene;
			currentScene = SceneManager.GetActiveScene;
		}
		*/
	}
}
