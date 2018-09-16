using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MenuCamera : MonoBehaviour {
	private int menuId = 0;
	private float destX = 0;
	private float destY	 = 0;
	private Scene currentScene;
	void Update(){
		currentScene = SceneManager.GetActiveScene ();
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
			destX = -15f;
		}
		Vector3 position = transform.position;
		position.y += (destY - position.y)*0.1f;
		position.x += (destX - position.x)*0.1f;
		//position.z=-90f;
		transform.position = position;
		//}
		//Transition to game scene
		if(position.y > 19f){
			SceneManager.LoadScene("sn_game");
		}
		//Transition to wiki scene
		if(position.x < -13f){
			SceneManager.LoadScene("Wiki");
		}
	}
	public void changeMenu(int id){
		menuId = id;
	}	
}

	