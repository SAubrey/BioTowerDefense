using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Game : MonoBehaviour {
<<<<<<< HEAD
	public static bool game;
=======
	public static bool game = false; // True so long as there is a running game state, paused or not. False when gameOver
	public static bool paused = false;
>>>>>>> e7d23a07a808e65c647d10d40720503616ba7a3c
	public GameObject[] waypoints;
	
	public int HP;
	public Text HPText;
	public Text GameOverText;
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
			if (!paused) {

			}
		}
		
		if (gameOver) {
			GameOverText.text = "GAME OVER";
			Gray.SetActive(true);
<<<<<<< HEAD
		}
		else{
			GameOverText.text = "";
			Gray.SetActive(false);
		}
		if(HP <=0 && game){
			var audioObject = GameObject.Find("AudioObject");
			audioObject.GetComponent<AudioSource>().clip = Resources.Load("Sounds/death") as AudioClip;
			audioObject.GetComponent<AudioSource>().Play();
			game = false;
			gameOver = true;
			app.GetComponent<MusicPlayer>().NewSong("death");
		}
=======
		} 
	}

	private void endGame() {
		var audioObject = GameObject.Find("AudioObject");
		audioObject.GetComponent<AudioSource>().clip = Resources.Load("Sounds/lose") as AudioClip;
		audioObject.GetComponent<AudioSource>().Play();
		game = false;
		gameOver = true;
		app.GetComponent<MusicPlayer>().NewSong("death");
>>>>>>> e7d23a07a808e65c647d10d40720503616ba7a3c
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
	
}