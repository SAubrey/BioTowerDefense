using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class scr_game_cam : MonoBehaviour {
	private float destX = 0f;
	private float destY = 0f;
	// Use this for initialization
	void Start () {
		Vector3 position = transform.position;
		position.y = -10f;
		transform.position = position;
	}
	
	// Update is called once per frame
	void Update () {
		Vector3 position = transform.position;
		position.y += (destY - position.y)*0.1f;
		position.x += (destX - position.x)*0.1f;
		transform.position = position;
		if(position.y < -14){
			SceneManager.LoadScene("MainMenu");
		}
	}
	
	public void Quit(){
		destY = -15f;
	}
}
