using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Tower : MonoBehaviour {

    public string towerName;
	public string antibioticType;
	public int type = 0; // 0 = pellet, 1 = laser, 2 = AOE 
    public int cost;
	public float coolDown = 0f; 
	//public int targetType = 0;//0=first, 1=last, 2=lowestHP, 3=highestHP, 4=self(aoe), 5=all in radius
	public float detectionRadius;
	//public Sprite projectileSprite;
	public float projectileSize = 0.5f;
	public float projectileSpeed = 0.6f;
	public int projectilePierce = 1;
	// public int specialEffect = 0; //0 = none, 1 = slow, 2 = increase damage taken, etc.
    private GameObject target = null;
    private GameObject projectile;
	private TowerManager towerManager;
    bool coolingDown = false;
    float cdTime = 0f;
	private IDictionary<string, float> baseDamages;

    void Start () {
		projectile  = Resources.Load("Prefabs/Projectile") as GameObject;
		towerManager = GameObject.Find("Game").GetComponent<TowerManager>();
		baseDamages = __app.antibiotics[antibioticType];
    }
	
	void Update () {
		// Return if not actually placed
		if (tag == "MenuItems") {
			return;
		}
		// Return if paused or game not in session
		if (Game.paused || !Game.game) {
			return;
		}
		// When cooled down, attack!
		if (!coolingDown) {
			activate();
		}
		else {
			if (cdTime <= 0f) {
				coolingDown = false;
			}
			cdTime -= Time.deltaTime;
		}
	}

	// From all existing enemies, a list of harmable enemies is found. 
	// (Within detection radius & non-zero effectiveness)
	// The first most harmable enmy from this list is chosen.
    private GameObject findTarget() {
		var enemies = GameObject.FindGameObjectsWithTag("Enemy");
		GameObject bestCandidate = null;
		float bestDamage = 0f;

		foreach (var enemy in enemies) {
			// Get distance
			var dist = Mathf.Sqrt(Mathf.Pow(enemy.transform.position.x - transform.position.x, 2f) +
									 Mathf.Pow(enemy.transform.position.y - transform.position.y, 2f));

			if (dist <= detectionRadius) {
				var enemyScript = enemy.GetComponent<Enemy>();
				float damage = baseDamages[enemyScript.species];

				// If tower can do damage to the enemy && If the enemy has not mutated against tower
				if (damage > 0 && !enemyScript.checkResistance(antibioticType)) { 
					if (damage > bestDamage) { 
						bestCandidate = enemy;
						bestDamage = damage;
						if (damage == 1) { // No need to continue if we found a prime target
							break;
						}
					}
				}
			}
		}
		if (bestCandidate != null) {
			print(antibioticType + " targeting: " + bestCandidate.GetComponent<Enemy>().species + " Best damage: " + bestDamage);
		}
		return bestCandidate;
	}
	
	private void activate() {
		if (type == 0 || type == 1) { 
			if (target == null) {
				target = findTarget();
			}
			if (target != null) { 
				//Check if target is out of bounds
				var targetDist = Mathf.Sqrt(Mathf.Pow(target.transform.position.x - transform.position.x, 2f) + 
											Mathf.Pow(target.transform.position.y - transform.position.y, 2f));
				if (targetDist > detectionRadius) {
					target = null;
				}
				else {
					if (type == 0) { // PELLET
						shoot(target);
					}
					else if(type == 1) { // LASER
						target.GetComponent<Enemy>().hurt(5, this);
					}
				}
			}
		}
		else if (type == 2) { // AOE
			var enemies = GameObject.FindGameObjectsWithTag("Enemy");
			var objectCount = enemies.Length;

			foreach (var enemy in enemies) {
				//Get Distance
				var dist = Mathf.Sqrt(Mathf.Pow(enemy.transform.position.x - transform.position.x, 2f) +
										 Mathf.Pow(enemy.transform.position.y - transform.position.y, 2f));
				if (dist <= detectionRadius) {
					enemy.GetComponent<Enemy>().hurt(5, this);
				}
			}
		}
		coolingDown = true;
		cdTime = coolDown;
	}
	
	private void shoot(GameObject enemy) {
		if (type == 0) {
			GameObject myProjectile = Instantiate(projectile);
			myProjectile.transform.position = new Vector3(transform.position.x, 
														transform.position.y, transform.position.z);
			var run = enemy.transform.position.x - transform.position.x;
			var rise = enemy.transform.position.y - transform.position.y;
			var distance = Mathf.Sqrt(Mathf.Pow(run, 2f) + Mathf.Pow(rise, 2f));
			var xsp = (run / distance) * projectileSpeed;
			var ysp = (rise / distance) * projectileSpeed;
			myProjectile.GetComponent<Projectile>().setVals(this, projectileSize, 
															 projectilePierce, xsp, ysp);
		}
	}

	// Called by an Enemy that has mutated against this tower.
	public void decoupleTarget() {
		target = null;
	}
	
	//Placement stuff
	    private void OnMouseDown() {
    	if (gameObject.tag != "MenuItems" && !Game.paused) {
			towerManager.destroyCircle();
			towerManager.lineRenderer = gameObject.GetComponent<LineRenderer>();
			towerManager.SelectedTower = gameObject;
			towerManager.drawCircle(detectionRadius);
			towerManager.setLabels(towerName, cost);
			towerManager.enableSellButton();
		}
	}
	
}
