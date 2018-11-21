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
	public float projectileSize = 0.5f;
	public float projectileSpeed = 0.6f;
	public int projectilePierce = 1;
    private GameObject target = null;
    private GameObject projectile;
	private TowerManager towerManager;
    bool coolingDown = false;
    float cdTime = 0f;
	private IDictionary<string, float> baseDamages;
	private float bestDamage = 0f;
	private Color color;
	private __app appScript;
	//Testing
	public int ammo = 5;

	// Laser specific
	private LineRenderer lr;
	private bool firingLaser = false;

	private LayerMask enemyMask;

    void Start () {
		projectile  = Resources.Load("Prefabs/Projectile") as GameObject;
		towerManager = GameObject.Find("Game").GetComponent<TowerManager>();
		baseDamages = __app.antibiotics[antibioticType];
		color = __app.colors[antibioticType];
		enemyMask = LayerMask.GetMask("Enemy");

		if (type == 1) {
			lr = gameObject.transform.GetChild(1).GetComponent<LineRenderer>();
			lr.positionCount = 2;
			lr.startColor = color;
			lr.endColor = color;
			lr.SetPosition(0, new Vector3(0, 0, 0));
		}
		if (type == 2) {
			appScript = GameObject.Find("__app").GetComponent<__app>();
		}
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
			cdTime -= Time.deltaTime;
			if (cdTime <= 0f) {
				coolingDown = false;
			}
			if (firingLaser) {
				updateLaser();
			}
		}
	}

	private void updateLaser() {
		Color c = lr.startColor;

		// Decrease alpha of laser color until gone, then reset.
		if (c.a > 0) {
			Color color = new Color(c.r, c.g, c.b, c.a - 0.05f);
			lr.startColor =color;
			lr.endColor = color;
		} else {
			firingLaser = false;
			lr.enabled = false;
		}
	}

	private GameObject findTarget() {
		Collider2D[] enemies = Physics2D.OverlapCircleAll((Vector2)gameObject.transform.position, detectionRadius, enemyMask);
		GameObject bestCandidate = null;
		bestDamage = 0f;

		foreach (Collider2D enemy in enemies) {
			Enemy enemyScript = enemy.GetComponent<Enemy>();
			float damage = baseDamages[enemyScript.species];

			// If tower can do damage to the enemy && If the enemy has not mutated against tower
			if (damage > bestDamage && !enemyScript.checkResistance(antibioticType)) { 
				bestCandidate = enemy.gameObject;
				bestDamage = damage;
				if (damage == 1) { // No need to continue if we found a prime target
					break;
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
			
			// If there's no target or if the target could be better, re-search.
			if (target == null || bestDamage != 1f) {
				target = findTarget();
			}
			if (target != null) { 
				// Check if target is out of bounds
				var targetDist = Mathf.Sqrt(Mathf.Pow(target.transform.position.x - transform.position.x, 2f) + 
											Mathf.Pow(target.transform.position.y - transform.position.y, 2f));
				if (targetDist > detectionRadius) {
					decoupleTarget();
				}
				else {
					if (type == 0) { // PELLET
						shoot(target);
					}
					else if(type == 1) { // LASER
						fireLaser(target);
					}
				}
			}
			ammo--;
			if(ammo<1){
				Destroy(gameObject);
			}
		}
		else if (type == 2) { // AOE
			explode();
		}
		coolingDown = true;
		cdTime = coolDown;
	}

	private void explode() {
		Collider2D[] col = Physics2D.OverlapCircleAll((Vector2)gameObject.transform.position, detectionRadius, enemyMask);

		bool boom = false;
		foreach (Collider2D c in col) {
			c.GetComponent<Enemy>().hurt(5, this);
			if (!boom) { boom = !boom; }
		}
		if (boom) {
			// Visual
			appScript.explode(gameObject.transform.position, 10, .1f, color);
		}
	}

	private void fireLaser(GameObject enemy) {
	
		// Physical
		RaycastHit2D[] rc;
		rc = Physics2D.LinecastAll(transform.position, target.transform.position, enemyMask);
		foreach (RaycastHit2D r in rc) {
			r.collider.gameObject.GetComponent<Enemy>().hurt(5, this);
		}

		// Visual
		Vector3 tPos = new Vector3(enemy.transform.position.x - transform.position.x, 
								enemy.transform.position.y - transform.position.y, 2f);
		firingLaser = true;
		if (!lr.enabled) {
			lr.enabled = true;
		}
		lr.startColor = color;
		lr.endColor = color;
		lr.SetPosition(1, tPos);
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
		bestDamage = 0;
	}

	// Placement stuff
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