using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Game : MonoBehaviour {
	public static bool game = false; // True so long as there is a running game state, paused or not. False when gameOver
	public static bool paused = false;
	public GameObject[] waypoints;
	
	public int HP;
	public Text HPText;
	public Text GameOverText;
    public Text currencyText;
	public GameObject Gray;
	public static bool gameOver = false;
	private GameObject app;


	void Start () {
		game = false;
		gameOver = false;
		app = GameObject.Find("__app");
		HPText.text = "HP: " + HP;
		GameOverText.text = "";
		Gray.SetActive(false);
        Currency = 150;
	}
	
	// Use this for initialization
	public void startGame () {
		game = true;
		paused = false;
		gameOver = false;
		HPText.text = "HP: " + HP;
		GameOverText.text = "";
		Gray.SetActive(false);
	}
	
	// Update is called once per frame
	void Update () {
		if (game) {
			//if (!paused) {
			//}

			if (HP <= 0) {
				endGame();
			}
		}
	}

	private void endGame() {
		GameOverText.text = "GAME OVER";
		Gray.SetActive(true);
		var audioObject = GameObject.Find("AudioObject");
		audioObject.GetComponent<AudioSource>().clip = Resources.Load("Sounds/lose") as AudioClip;
		audioObject.GetComponent<AudioSource>().Play();
		game = false;
		gameOver = true;
		app.GetComponent<MusicPlayer>().NewSong("death");
	}
	
	public void takeDamage(int damage){
		HP -= damage;
		HPText.text = "HP: " + HP;

		if (HP <= 0) {
			endGame();
		}
	}

	public void pauseGame() {
		paused = true;
	}

    private int _currency;
    public int Currency {
        get {
            return _currency;
        }
        set {
            _currency = value;
            currencyText.GetComponent<Text>().text = "$: " + _currency;
        }
    }
}