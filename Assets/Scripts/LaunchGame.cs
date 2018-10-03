using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class LaunchGame : MonoBehaviour {

	// Use this for initialization
	void Start () {
		GetComponent<SceneGuy>().ChangeScene("MainMenu");
	}
}
