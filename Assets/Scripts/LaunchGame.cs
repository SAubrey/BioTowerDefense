using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class LaunchGame : MonoBehaviour {
	
	void Start () {
		GetComponent<SceneGuy>().ChangeScene("MainMenu");
	}
}
