using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;

public class EnemyManager : MonoBehaviour {

	private Game game;
	private float spawnTimer = 0f;
	public float spawnInterval = .9f; // seconds
	private GameObject pneu;
	private GameObject staph;
	private GameObject strep;
	private GameObject TB;
	private bool burstSpawn = false;
	private float burstTimer = 0f;
	public float burstInterval = 4f;
	private int burstEnemiesRemaining = 0;
	public int burstEnemyCount = 4; // Number of same enemy spawned in a row.
	private GameObject burstEnemy;

	public int[] wavesEnemyCounts = {9, 12, 16, 19, 21, 23, 25, 27, 29, 31};
	public int currentWave = 0;
	private int enemiesSpawnedInWave = 0;
	private bool waveActive = true;
	public float waveInterval = 8f;
	private float waveIntervalTimer = 0f;
	public Text EnemyText;
	public Text TimerText; 	
	public float enemiesKilled;
	
	// Use this for initialization

	void Start () {
		game = GameObject.Find("Game").GetComponentInParent<Game>();

		pneu = Resources.Load("Prefabs/Enemies/Pneumonia") as GameObject;
		staph = Resources.Load("Prefabs/Enemies/Staph") as GameObject;
		strep = Resources.Load("Prefabs/Enemies/Strep") as GameObject;
		TB = Resources.Load("Prefabs/Enemies/TB") as GameObject;
	}
	
	// Update is called once per frame
	 
	void Update () {
		if (Game.game) {
			if (!Game.paused) {
				manageSpawn(Time.deltaTime);
			}
			EnemyText.text = "Enemies: "+ (wavesEnemyCounts[currentWave] - enemiesKilled);
			TimerText.text = "Timer: "+Mathf.RoundToInt(waveIntervalTimer) + "/" + waveInterval;
		}
	}

	private void manageSpawn(float deltaTime) {
		if (!waveActive) {
			waveIntervalTimer += deltaTime;
			if (waveIntervalTimer >= waveInterval) {
				progressWave();
			}
			return;
		} 

		burstTimer += deltaTime;
		if (burstTimer >= burstInterval) {
			initiateBurst();
		}

		// Special spawning is still bound by the general spawn timer.
		spawnTimer += deltaTime;
		if (spawnTimer >= spawnInterval) {
			if (burstSpawn) {
				spawnBurstEnemy();
			} 
			else {
				spawnEnemy(chooseRandomEnemy(25, 25, 25, 25));
			}
			spawnTimer = 0;
		}
	}

	private void initiateBurst() {
		burstSpawn = true;
		burstEnemy = chooseRandomEnemy(25, 25, 25, 25);
		burstEnemiesRemaining = burstEnemyCount;
		burstTimer = 0;
		print("Burst initiated for " + burstEnemyCount + " enemies.");
	}
	private void spawnBurstEnemy() {
		spawnEnemy(burstEnemy);
		burstEnemiesRemaining--;
		if (burstEnemiesRemaining <= 0) {
			burstSpawn = false;
		}
	}
	private void spawnEnemy(GameObject Enemy) {
        // Instantiate(Enemy, new Vector3(-10.5f,3.25f,0f),Quaternion.identity);
        GameObject newEnemy = Instantiate(Enemy);
        newEnemy.GetComponent<Enemy>().waypoints = game.waypoints;

		enemiesSpawnedInWave++;
		if (enemiesSpawnedInWave >= wavesEnemyCounts[currentWave]) {
			waveActive = false;
		}
    }

	private GameObject chooseRandomEnemy(int pneuWeight, int staphWeight, int strepWeight, int TBWeight) {
		int choice = Random.Range(0, pneuWeight + staphWeight + strepWeight + TBWeight); // Should equal 0-100
		GameObject enemy = pneu;
		if (choice <= pneuWeight) { 
			enemy = pneu;
		} 
		else if (choice <= pneuWeight + staphWeight) {
			enemy = staph;
		}
		else if (choice <= pneuWeight + staphWeight + strepWeight) {
			enemy = strep;
		}
		else if (choice <= pneuWeight + staphWeight + strepWeight + TBWeight) {
			enemy = TB;
		}
		return enemy;
	}

	// Called after waveInterval has been reached.
	private void progressWave() {
		// Reset values
		waveIntervalTimer = 0;
		enemiesSpawnedInWave = 0;
		spawnTimer = 0;
		burstTimer = 0;
		burstEnemiesRemaining = 0;
		burstSpawn = false;
		enemiesKilled = 0;

		// Advance and toggle spawning.
		currentWave++;
		waveActive = true;
		print("Beginning wave " + currentWave);
	}
}
