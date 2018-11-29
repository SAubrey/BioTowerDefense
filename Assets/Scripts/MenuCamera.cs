using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MenuCamera : MonoBehaviour {
	private GameObject app;
	private string scene;
	private string previousScene;
	private string nextScene;
	private string nextScreen;
	private float cameraHeight = -20.79f;
	private SceneGuy sceneGuy;
	private bool sceneChange = true;
	private IDictionary<string, Vector2> mainMenuCoordinates;
	private Vector2 dest;
	private bool moving = true;
	private float xOffset;
	private float yOffset;
	private __app appScript;

	void Start() {
		mainMenuCoordinates = new Dictionary<string, Vector2>() {
			{"MainMenu", new Vector2(0f, 0f)},
			{"PlayMenu", new Vector2(0f, 10f)},
			{"Game", new Vector2(0f, 30f)},
			{"OptionsMenu", new Vector2(0f, -10f)},
			{"Encyclopedia", new Vector2(-30f, 0f)} ,
			{"labMain", new Vector2(0f, 0f)},
			{"labData", new Vector2(0f, 10f)},
			{"labLogbook", new Vector2(-17.8f, 0f)},
		};

		app = GameObject.Find("__app");
		appScript = app.GetComponent<__app>();
		sceneGuy = app.GetComponent<SceneGuy>();
		scene = sceneGuy.currentScene;
		previousScene = sceneGuy.previousScene;
		//screen = scene;
		determineStartPos();
		xOffset = 0;
		yOffset = 0;

		if (scene == "MainMenu") {
			GameObject.Find("Strep").GetComponent<Animator>().Play("strep");
			GameObject.Find("Staph").GetComponent<Animator>().Play("staph");
			GameObject.Find("TB").GetComponent<Animator>().Play("TB");
			GameObject.Find("Pneu").GetComponent<Animator>().Play("pneu");
		} 
		else if ( scene == "Encyclopedia") {

		}
	}

	void Update() {
		if (moving) {
			moveCamera();
		}
		if(appScript.isScreenshaking()){
			//print("SHAKING");
			xOffset = appScript.getXOffset();
			yOffset = appScript.getYOffset();
			transform.position = new Vector3(transform.position.x + xOffset,transform.position.y + yOffset,transform.position.z);
		}
		else{
			xOffset = 0;
			yOffset = 0;
		}	
	}

	private void determineStartPos() {
		if (previousScene == "MainMenu") {
			if (scene == "Encyclopedia") {
				transform.position = new Vector3(20, 0, cameraHeight);
			}
			else if (scene == "Game") {
				transform.position = new Vector3(0, -20, cameraHeight);
			}
		}
		else if (previousScene == "Encyclopedia") {
			transform.position = new Vector3(-20, 0, cameraHeight);
		}
		else if (previousScene == "Game") {
			if (scene == "MainMenu") {
				transform.position = new Vector3(0, 20, cameraHeight);
			}
		}
	}

	/*
		sceneChanged is only false when moving to the new scene,
		but before the new scene has been loaded. 
	 */
	private void moveCamera() {
		Vector3 position = transform.position;
		float dy = dest.y - position.y;
		float dx = dest.x - position.x;
		position.y += dy * 0.1f;
		position.x += dx * 0.1f;
		transform.position = position;

		if (sceneChange) {
			// print("scene: " + scene + " nextScene: " + nextScene);
			if (scene == "MainMenu") {
				if (nextScene == "Encyclopedia") {
					if (position.x < -20f) {
						sceneGuy.ChangeScene(nextScene);
					}
				}
				else if (nextScene == "Game") {
					if (position.y > 20) {
						sceneGuy.ChangeScene(nextScene);
					}
				}
			}
			else if (scene == "Encyclopedia") {
				if (nextScene == "MainMenu") {
					if (position.x > 20f) {
						sceneGuy.ChangeScene(nextScene);
					}
				}
			}
			else if (scene == "Game") {
				if (nextScene == "MainMenu") {
					if (position.y < -20) {
						sceneGuy.ChangeScene(nextScene);
					}
				}
			}
			
		} else {
			// If movement distance for frame is near zero, stop moving.
			if (Math.Abs(dx) < .02 && Math.Abs(dy) < .02) {
				moving = false;
			}
		}
	}
	
	// Called by menu buttons
	public void changeScreen(string scr) {

		Time.timeScale = 1f;
		//if(sceneGuy.currentScene == "MainMenu")
			dest = mainMenuCoordinates[scr]; // Gives coordinates if scene is MainMenu
		//if(sceneGuy.currentScene == "Encyclopedia")
			//dest = labMenuCoordinates[scr]; // Gives coordinates if scene is LabMenu
		setSceneFromScreen(scr);
		moving = true;
	}

	/*
	Changing screens (menus) does not always mean changing scenes.
	 */
	private void setSceneFromScreen(string scr) {
		nextScene = scr;
		scene = SceneManager.GetActiveScene().name;
		if (scr == "MainMenu" || scr == "PlayMenu" || scr == "OptionsMenu") {
			nextScene = "MainMenu";
			if (scene == "Encyclopedia") { // if in Encyclopedia moving to main menu
				dest.x = 30f;
				dest.y = 0f;
			} 
			else if (scene == "Game") {
				dest.x = 0f;
				dest.y = -30f;
			}
		}

		if (nextScene != scene) {
			sceneChange = true;
		}
	}
}