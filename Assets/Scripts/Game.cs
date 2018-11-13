using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Game : MonoBehaviour {
	public static bool game = false; // True so long as there is a running game state, paused or not. False when gameOver
	public static bool paused = false;
	[HideInInspector] 
	public GameObject[] waypoints;
	
	public int HP;
	public Text HPText;
	public Text GameOverText;
	private string _gameOverText = "";
    public Text currencyText;
	public Text waveText;
	public GameObject Gray;
	public static bool gameOver = false;
	private GameObject app;
	private __app appScript;
	private GameObject level;
	public float timescale;

	void Start () {
		game = false;
		timescale = 1f;
		gameOver = false;
		app = GameObject.Find("__app");
		appScript = app.GetComponent<__app>();
		HPText.text = "HP: " + HP;
		gameOverText = "";
        Currency = 150;

		// Load Level
		level = Resources.Load("Prefabs/Levels/"+appScript.getLevel()) as GameObject;
		Instantiate(level);

		// Find and load waypoints
		GameObject[] foundWaypoints = GameObject.FindGameObjectsWithTag("Waypoint");
		waypoints = new GameObject[foundWaypoints.Length];
		// Debug.Log("Waypoints: " + foundWaypoints.Length.ToString());
		for (int i = 0; i<foundWaypoints.Length; i++) {
			foreach (GameObject way in foundWaypoints) {
				if (way.name == "Waypoint" + i.ToString()) {
					waypoints[i] = way;
				}
			}
		}
	}
	
	// Called by start button
	public void startGame () {
		if (HP > 0) {
			game = true;
			paused = false;
		}
	}
	
	void Update () {
		/* 
		if (game) {
			//if (!paused) {
			//}
		}
		*/
	}

	private void endGame() {
		gameOverText = "GAME OVER";
		var audioObject = GameObject.Find("AudioObject");
		audioObject.GetComponent<AudioSource>().clip = Resources.Load("Sounds/lose") as AudioClip;
		if (appScript.getSFX()) {
			audioObject.GetComponent<AudioSource>().Play();
		}
		game = false;
		gameOver = true;
		app.GetComponent<MusicPlayer>().NewSong("death");
	}

	public void passLevel() {
		gameOverText = "YOU WIN!";
		game = false;
		gameOver = true;
/*
		var audioObject = GameObject.Find("AudioObject");
		audioObject.GetComponent<AudioSource>().clip = Resources.Load("Sounds/win") as AudioClip;
		if (appScript.getSFX()) {
			audioObject.GetComponent<AudioSource>().Play();
		}
*/
	}
	
	public void takeDamage(int damage){
		HP -= damage;
		HPText.text = "HP: " + HP;

		if (HP <= 0) {
			endGame();
		}
	}

	public void togglePaused() {
		paused = !paused;
	}
	
	public void toggleTimescale() {
		if (timescale == 1f){
			timescale = 2f;
		}
		else {
			timescale = 1f;
		}
	}

    private int _currency;
    public int Currency {
        get {
            return _currency;
        }
        set {
            _currency = value;
            currencyText.GetComponent<Text>().text = "$" + _currency;
        }
    }

	public string gameOverText {
		get {
			return _gameOverText;
		}
		set {
			_gameOverText = value;
			if (value == "") {
				GameOverText.text = "";
				Gray.SetActive(false);
			} else {
				GameOverText.text = value;
				Gray.SetActive(true);
			}
		}
	}
}