using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CheckPreLoad : MonoBehaviour {

	///https://stackoverflow.com/questions/35890932/unity-game-manager-script-works-only-one-time/35891919#35891919
	// this should run absolutely first; use script-execution-order to do so.
	// (of course, normally never use the script-execution-order feature,
	// this is an unusual case, just for development.)
	 void Awake()
	  {
	  GameObject check = GameObject.Find("__app");
	  if (check==null)
	   { UnityEngine.SceneManagement.SceneManager.LoadScene("_preload"); }
	  }
}
