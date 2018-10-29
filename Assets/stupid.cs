using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class stupid : MonoBehaviour {

	public void reallyStupid(string levelName){
		GameObject.Find("__app").GetComponent<__app>().setLevel(levelName);
	}
}
