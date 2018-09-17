using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class SceneGuy : MonoBehaviour {
	public int test;
	public string currentScene;
	public string previousScene;
	// Use this for initialization
	void Start () {
		//currentScene = SceneManager.GetActiveScene ().name;
		//previousScene = SceneManager.GetActiveScene ().name;
	}
	
	// Update is called once per frame
	void Update () {
		//Scene Management
		var checkScene = SceneManager.GetActiveScene().name;
		if(checkScene != currentScene){
			Debug.Log("TRANSITION DETECTED!");
			Debug.Log("Previous Scene: "+currentScene);
			previousScene = currentScene;
			currentScene = SceneManager.GetActiveScene().name;
			Debug.Log("Current Scene: "+currentScene);
		}
		if (Input.GetKey(KeyCode.D)){
            Debug.Log("CHECK PREV: "+previousScene);
        }
		if (Input.GetKey(KeyCode.E)){
            Debug.Log("CHECK CURRENT: "+currentScene);
        }
		if (Input.GetKey(KeyCode.S)){
            test++;
			Debug.Log(test);
        }
		
	}
	
	public void ChangeScene(string name){
		SceneManager.LoadScene(name);
		//Change Music
		if(name == "Wiki"){
			GetComponent<MusicPlayer>().NewSong("science4");
		}
		if(name == "MainMenu"){
			GetComponent<MusicPlayer>().NewSong("science2");
		}
		if(name == "Game"){
			GetComponent<MusicPlayer>().NewSong("science");
		}
	}
}
