using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class scr_quitGame : MonoBehaviour {

	public void quitGame(){
		var objects = GameObject.FindGameObjectsWithTag("MainCamera");
			var objectCount = objects.Length;
			foreach (var obj in objects) {
				obj.GetComponent<scr_game_cam>().Quit();
			}
	}
}
