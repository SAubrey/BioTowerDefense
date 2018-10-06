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
	private string screen;
	private string nextScreen;
	private float cameraHeight = -20.79f;
	private SceneGuy sceneGuy;
	private bool sceneChange = true;
	private bool screenChange = true;
	private IDictionary<string, Vector2> mainMenuCoordinates;
	private Vector2 dest;
	private bool moving = true;
	void Awake() {
		
		
	}

	void Start() {
		mainMenuCoordinates = new Dictionary<string, Vector2>() 
													{
														{"MainMenu", new Vector2(0f, 0f)},
														{"PlayMenu", new Vector2(0f, 10f)},
														{"Game", new Vector2(0f, 30f)},
														{"OptionsMenu", new Vector2(0f, -10f)},
														{"Encyclopedia", new Vector2(-30f, 0f)}
													};
		app = GameObject.Find("__app");
		sceneGuy = app.GetComponent<SceneGuy>();
		scene = sceneGuy.currentScene;
		previousScene = sceneGuy.previousScene;
		screen = scene;
		determineStartPos();
	}

	void Update() {
		if (moving) {
			moveCamera();
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
				screenChange = false;
				moving = false;
			}
		}
	}
	
	// Called by menu buttons
	public void changeScreen(string scr) {
		screenChange = true;
		dest = mainMenuCoordinates[scr]; // Gives coordinates assuming scene is MainMenu
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