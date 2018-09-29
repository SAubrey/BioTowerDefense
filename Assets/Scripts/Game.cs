using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Game : MonoBehaviour {
	public static bool game = false;
	public GameObject[] waypoints;
	private int time = 0;
	private int spawnInterval = 30;
	public int HP;
	public Text HPText;
	public Text GameOverText;
	public GameObject Gray;
	public static bool gameOver = false;
	private GameObject app;
	void Start () {
		gameOver = false;
		app = GameObject.Find("__app");
	}
	
	// Use this for initialization
	public void StartGame () {
		game = true;
		gameOver = false;
	}
	
	// Update is called once per frame
	void Update () {
		if(game){
			if(time >= spawnInterval){
				GameObject enemy  = Resources.Load("Prefabs/Enemy") as GameObject;
				SpawnEnemy (enemy);
				time = 0;
			}
			time++;
		}
		HPText.text = "HP: "+HP;
		if(gameOver){
			GameOverText.text = "GAME OVER";
			Gray.SetActive(true);
		}
		else{
			GameOverText.text = "";
			Gray.SetActive(false);
		}
		if(HP <=0 && game){
			var audioObject = GameObject.Find("AudioObject");
			audioObject.GetComponent<AudioSource>().clip = Resources.Load("Sounds/lose") as AudioClip;
			audioObject.GetComponent<AudioSource>().Play();
			game = false;
			gameOver = true;
			app.GetComponent<MusicPlayer>().NewSong("death");
		}
	}
	
	public void TakeDamage(int damage){
		HP-=damage;
	}

	public void SpawnEnemy(GameObject Enemy){
        //Instantiate(Enemy, new Vector3(-10.5f,3.25f,0f),Quaternion.identity);
        GameObject newEnemy = Instantiate(Enemy);
        newEnemy.GetComponent<Enemy>().waypoints = waypoints;
    }
	
	/*public void QuitGame() {
		var Camera = GameObject.Find("Main Camera");
		Camera.GetComponent<GameCam>().Quit();
		game = false;
	}*/
}
