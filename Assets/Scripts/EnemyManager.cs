using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using System.Collections.Generic;

public class EnemyManager : MonoBehaviour {

	private Game game;
	private float spawnTimer = 0f;
	public float spawnInterval;
	public GameObject Enemy;

	// Burst Spawning Mode Management
	private bool burstSpawn = false;
	private float burstTimer = 0f;
	public float burstInterval = 4f;
	private int burstEnemiesRemaining = 0;
	public int burstEnemyCount = 4; // Number of same enemy spawned in a row.
	private string burstEnemy;

	// Wave Management
	public int[] wavesEnemyCounts = {0, 9, 12, 16, 19, 21, 23, 25, 27, 29, 31};
	private int _currentWave = 1;
	private int enemiesSpawnedInWave = 0;
	private bool waveActive = false;

	private bool spawningActive = true;
	private Text EnemyText;
	private int _enemiesDead = 0;
	public int waveCompleteReward = 20; 
	public float waveCompleteMutMult = 0.5f;

	void Start () {
		game = GameObject.Find("Game").GetComponentInParent<Game>();
		EnemyText = GameObject.Find("Enemies Text").GetComponent<Text>();
		enemiesDead = 0;
		currentWave = 0;

		spawnInterval = __app.spawnInterval;
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
		
			game.startButton.SetActive(true);

			if (game.startNextWave) {
				progressWave();
				game.startNextWave = false;
				game.startButton.SetActive(false);
			}
			return;
		} 
		else if (spawningActive) {
			burstTimer += deltaTime;
			if (burstTimer >= burstInterval) {
				initiateBurst(burstEnemyCount);
			}

			// Special spawning is still bound by the general spawn timer.
			spawnTimer += deltaTime;
			if (spawnTimer >= spawnInterval) {
				if (burstSpawn) {
					spawnBurstEnemy();
				} 
				else {
					spawnEnemy(chooseRandomEnemy(27, 27, 27, 19));
				}
				spawnTimer = 0;
			}
		}
	}

	private void initiateBurst(int count) {
		burstSpawn = true;
		burstEnemy = chooseRandomEnemy(26, 26, 26, 21);
		burstEnemiesRemaining = count;
		burstTimer = 0;
		print("Burst initiated for " + count + " enemies.");
	}
	private void spawnBurstEnemy() {
		spawnEnemy(burstEnemy);
		burstEnemiesRemaining--;
		if (burstEnemiesRemaining <= 0) {
			burstSpawn = false;
		}
	}
	private void spawnEnemy(string enemyName) {
		GameObject enemy = Instantiate(Enemy);
		enemy.GetComponent<Enemy>().waypoints = game.waypoints;		
		enemy.GetComponent<Enemy>().setSpecies(enemyName);

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
		enemiesSpawnedInWave = 0;
		spawnTimer = 0;
		burstTimer = 0;
		burstEnemiesRemaining = 0;
		burstSpawn = false;
		currentWave++;
		enemiesDead = 0; // set after inc wave

		// Advance and toggle spawning.
		waveActive = true;
		spawningActive = true;

		if (currentWave > 1) {
			GameObject.Find("__app").GetComponent<__app>().lowerAllChances(waveCompleteMutMult);
		}
	}

	public void incEnemiesDead() {
		enemiesDead++;

		if (enemiesDead >= wavesEnemyCounts[currentWave]) {

			waveActive = false;
            game.Currency += waveCompleteReward;
			
			if (currentWave >= wavesEnemyCounts.Length - 1) { // Win condition
				game.passLevel();
			}
        }
    }

    public int currentWave {
        get {
            return _currentWave;
        }
        set {
            _currentWave = value;
            game.waveText.GetComponent<Text>().text = "Wave: " + _currentWave;
        }
    }

	public int enemiesDead {
        get {
            return _enemiesDead;
        }
        set {
            _enemiesDead = value;
            EnemyText.GetComponent<Text>().text = "Enemies: " + (wavesEnemyCounts[currentWave] - _enemiesDead);
        }
    }
	
}