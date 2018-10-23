using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class SceneGuy : MonoBehaviour {
	public int test;
	public string currentScene;
	public string previousScene;

	private bool sceneChanged = true;
	// Use this for initialization
	void Start () {
		currentScene = SceneManager.GetActiveScene().name;
		previousScene = "";
	}
	
	// Update is called once per frame
	void Update () {
		if (sceneChanged) {
			sceneChanged = false;
			//Debug.Log("TRANSITION DETECTED!");
			//Debug.Log("Previous Scene: "+previousScene);
			//Debug.Log("Current Scene: "+currentScene);
		}

		if (Input.anyKey) { // reduces # checks
			if (Input.GetKey(KeyCode.D)) {
				Debug.Log("CHECK PREV: "+previousScene);
			}
			else if (Input.GetKey(KeyCode.E)) {
				Debug.Log("CHECK CURRENT: "+currentScene);
			}
			else if (Input.GetKey(KeyCode.S)) {
				test++;
				Debug.Log(test);
			}
		}
	}
	
	public void ChangeScene(string scene) {
		if (scene != currentScene) {
			SceneManager.LoadScene(scene);
			previousScene = currentScene;
			currentScene = scene;
			sceneChanged = true;

			//Change Music
			if(scene == "Encyclopedia"){
				GetComponent<MusicPlayer>().NewSong("science4");
			}
			else if(scene == "MainMenu"){
				GetComponent<MusicPlayer>().NewSong("science2");
			}
			else if(scene == "Game"){
				GetComponent<MusicPlayer>().NewSong("science");
			}
		}
	}
}
