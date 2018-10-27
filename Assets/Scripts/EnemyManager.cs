using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using System.Collections.Generic;

public class EnemyManager : MonoBehaviour {

	private Game game;
	private float spawnTimer = 0f;
	public float spawnInterval = .9f; // seconds
	public GameObject Enemy;
	public Sprite pneuSprite;
	public Sprite staphSprite;
	public Sprite strepSprite;
	public Sprite TBSprite;
	private IDictionary<string, Sprite> enemySprites;

	// Burst Spawning Mode Management
	private bool burstSpawn = false;
	private float burstTimer = 0f;
	public float burstInterval = 4f;
	private int burstEnemiesRemaining = 0;
	public int burstEnemyCount = 4; // Number of same enemy spawned in a row.
	private string burstEnemy;

	// Wave Management
	public int[] wavesEnemyCounts = {9, 12, 16, 19, 21, 23, 25, 27, 29, 31};
	public int currentWave = 0;
	private int enemiesSpawnedInWave = 0;
	private bool waveActive = true;
	private bool spawningActive = true;
	public float waveInterval = 8f;
	private float waveIntervalTimer = 0f;
	public Text EnemyText;
	public Text TimerText; 
	private float enemiesDead;

	void Start () {
		game = GameObject.Find("Game").GetComponentInParent<Game>();
		updateEnemyText();
		enemySprites = new Dictionary<string, Sprite>() {
												{"pneu", pneuSprite},
												{"staph", staphSprite}, 
												{"strep", strepSprite},
												{"TB", TBSprite} };
	}
	 
	void Update () {
		if (Game.game) {
			if (!Game.paused) {
				manageSpawn(Time.deltaTime);
			}
		}
	}

	private void manageSpawn(float deltaTime) {
		if (!waveActive) {
			waveIntervalTimer += deltaTime;
			TimerText.text = "Next Wave In: " + (Mathf.RoundToInt(waveInterval - waveIntervalTimer));
			if (waveIntervalTimer >= waveInterval) {
				progressWave();
				TimerText.text = "";
			}
			return;
		} 
		else if (spawningActive) {
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
	private void spawnEnemy(string enemyName) {
        // Instantiate(Enemy, new Vector3(-10.5f,3.25f,0f),Quaternion.identity);
		GameObject enemy = Instantiate(Enemy);
		enemy.GetComponent<Enemy>().waypoints = game.waypoints;		
        enemy.GetComponent<Enemy>().setSpecies(enemySprites[enemyName], enemyName);

		enemiesSpawnedInWave++;
		if (enemiesSpawnedInWave >= wavesEnemyCounts[currentWave]) {
			spawningActive = false;
		}
    }

	private string chooseRandomEnemy(int pneuWeight, int staphWeight, int strepWeight, int TBWeight) {
		int choice = Random.Range(0, pneuWeight + staphWeight + strepWeight + TBWeight); // Should equal 0-100
		
		if (choice <= pneuWeight) { 
			return "pneu";
		} 
		else if (choice <= pneuWeight + staphWeight) {
			return "staph";
		}
		else if (choice <= pneuWeight + staphWeight + strepWeight) {
			return "strep";
		}
		else if (choice <= pneuWeight + staphWeight + strepWeight + TBWeight) {
			return "TB";
		}
		return "pneu";
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
		enemiesDead = 0;


		// Advance and toggle spawning.
		currentWave++;
		waveActive = true;
		spawningActive = true;

		GameObject.Find("__app").GetComponent<__app>().lowerAllChances(0.5f);
		updateEnemyText();
		print("Beginning wave " + currentWave);
	}

	public void incEnemiesDead() {
		enemiesDead++;
		updateEnemyText();

		if (enemiesDead >= wavesEnemyCounts[currentWave]) {
			waveActive = false;
            game.Currency += 50;

        }
    }

	private void updateEnemyText() {
		EnemyText.text = "Enemies: "+ (wavesEnemyCounts[currentWave] - enemiesDead);
	}
}
