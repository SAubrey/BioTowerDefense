using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MenuCamera : MonoBehaviour {
	private int menuId = 0;
	private float destX = 0;
	private float destY	 = 0;
	private GameObject app;
	private string scene;
	private string oldscene;
	void Awake() {
		app = GameObject.Find("__app");
	}
	void Update(){
		//Check Scene Change
		var changed = false;
		if(scene != app.GetComponent<SceneGuy>().currentScene){
			changed = true;
		}
		//Get Scene
		scene = app.GetComponent<SceneGuy>().currentScene;
		oldscene = app.GetComponent<SceneGuy>().previousScene;
		if(changed){
			//Starting Camera Positions
			if(scene=="MainMenu"){
				if(oldscene=="Wiki"){
					Vector3 pos = transform.position;
					pos.y = 0;
					pos.x = -14;
					transform.position = pos;
				}
				if(oldscene=="Game"){
					Vector3 pos = transform.position;
					pos.y = 18;
					pos.x = 0;
					transform.position = pos;
					menuId = 1;
				}
			}
		}
		if(scene=="MainMenu"){
			//Main Menu
			if(menuId == 0){
				destY = 0f;
				destX = 0f;
			}
			//Play menu
			if(menuId == 1){
				destY = 10f;
				destX = 0f;
			}
			//Start Game
			if(menuId == 2){
				destY = 20f;
				destX = 0f;
			}
			//Options Menu
			if(menuId == 3){
				destY = -10f;
				destX = 0f;
			}
			//Wiki Menu
			if(menuId == 4){
				destY = 0f;
				destX = -20f;
			}
		}
		if(scene=="Wiki"){
			//Wiki
			if(menuId == 0){
				destY = 0f;
				destX = 0f;
			}
			//Return
			if(menuId == 1){
				destY = 0f;
				destX = 25;
			}
		}
		//Move Camera
		Vector3 position = transform.position;
		position.y += (destY - position.y)*0.1f;
		position.x += (destX - position.x)*0.1f;
		//position.z=-90f;
		transform.position = position;
		//}
		//Transition to game scene
		if(scene=="MainMenu"){
			if(position.y > 19f){
				app.GetComponent<SceneGuy>().ChangeScene("Game");
			}
			//Transition to wiki scene
			if(position.x < -15f){
				app.GetComponent<SceneGuy>().ChangeScene("Wiki");
			}
		}
		if(scene=="Wiki"){
			if(position.x > 22){
				app.GetComponent<SceneGuy>().ChangeScene("MainMenu");
			}
		}
	}
	public void changeMenu(int id){
		menuId = id;
	}	
}

	