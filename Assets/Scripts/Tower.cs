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
	public int targetType = 0;//0=first, 1=last, 2=lowestHP, 3=highestHP, 4=self(aoe), 5=all in radius
	public float detectionRadius;
	public Sprite projectileSprite;
	public float projectileSize = 0.5f;
	public float projectileSpeed = 0.6f;
	public int projectilePierce = 1;
	// public int specialEffect = 0; //0 = none, 1 = slow, 2 = increase damage taken, etc.
    private GameObject target = null;
    private GameObject projectile;
	private TowerManager towerManager;
    bool coolingDown = false;
    float cdTime = 0f;

    // Use this for initialization
    void Start () {
		projectile  = Resources.Load("Prefabs/Projectile") as GameObject;
		towerManager = GameObject.Find("Game").GetComponent<TowerManager>();
    }
	
	// Update is called once per frame
	void Update () {
		//Return if not actually placed
		if (tag == "MenuItems") {
			return;
		}
		//Return if paused or game not in session
		if (Game.paused || !Game.game) {
			return;
		}
		//Do this when not on Cooldown
		if (!coolingDown) {
			activate();
		}
		else {
			if (cdTime <= 0f) {
				coolingDown = false;
			}
			cdTime -= 1f * Time.timeScale;
		}
	}

    private void findTarget() {
		var enemies = GameObject.FindGameObjectsWithTag("Enemy");
		var objectCount = enemies.Length;

		foreach (var enemy in enemies) {
			//Get Distance
			var dist = Mathf.Sqrt(Mathf.Pow(enemy.transform.position.x - transform.position.x, 2f) +
									 Mathf.Pow(enemy.transform.position.y - transform.position.y, 2f));
			if (dist <= detectionRadius) {
				//Based on TargetType
				if( targetType == 0){
					target = enemy;
				}
				break;
			}
		}
	}
	
	private void activate() {
		if(type == 0 || type == 1){
			if (target == null) {
				findTarget();
			}
			else {
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
						// Debug.Log("BOOOOOOOOOOOOOOOOM");
						target.GetComponent<Enemy>().hurt(5, antibioticType);
					}
				}
			}
		}
		else if(type == 2) { // AOE
			var enemies = GameObject.FindGameObjectsWithTag("Enemy");
			var objectCount = enemies.Length;

			foreach (var enemy in enemies) {
				//Get Distance
				var dist = Mathf.Sqrt(Mathf.Pow(enemy.transform.position.x - transform.position.x, 2f) +
										 Mathf.Pow(enemy.transform.position.y - transform.position.y, 2f));
				if (dist <= detectionRadius) {
					enemy.GetComponent<Enemy>().hurt(5, antibioticType);
				}
			}
		}
		coolingDown = true;
		cdTime = coolDown;
	}
	
	private void shoot(GameObject enemy) {
		if(type==0){
			GameObject myProjectile = Instantiate(projectile);
			myProjectile.transform.position = new Vector3(transform.position.x, 
														transform.position.y, transform.position.z);
			var run = enemy.transform.position.x - transform.position.x;
			var rise = enemy.transform.position.y - transform.position.y;
			var distance = Mathf.Sqrt(Mathf.Pow(run, 2f) + Mathf.Pow(rise, 2f));
			var xsp = (run / distance) * projectileSpeed;
			var ysp = (rise / distance) * projectileSpeed;
			myProjectile.GetComponent<Projectile>().setVals(gameObject, antibioticType, projectileSize, 
															projectileSpeed, projectilePierce, xsp, ysp);
		}
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
