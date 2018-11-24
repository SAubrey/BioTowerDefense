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
	private GameObject bombAOE;
	private TowerManager towerManager;
    bool coolingDown = false;
    float cdTime = 0f;
	private IDictionary<string, float> baseDamages;
	private float bestDamage = 0f;
	private Color color;
	private __app appScript;
	public Image ammoBar;
	public GameObject ammoBarWhole;
	//Testing
	[HideInInspector]
	public int maxAmmo;
	[HideInInspector]
	public int ammo;

	// Laser specific
	private LineRenderer lr;
	private bool firingLaser = false;

	private LayerMask enemyMask;

	private Vector2 ellipseDetectionRadius;
	private LineRenderer myLr;

    void Start () {
		projectile  = Resources.Load("Prefabs/Projectile") as GameObject;
		bombAOE = Resources.Load("Prefabs/TowerAOE") as GameObject;
		towerManager = GameObject.Find("Game").GetComponent<TowerManager>();
		baseDamages = __app.antibiotics[antibioticType];
		color = __app.colors[antibioticType];
		enemyMask = LayerMask.GetMask("Enemy");

		if (tag == "MenuItems") {
			ammoBarWhole.SetActive(false);
		}
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

		ellipseDetectionRadius = new Vector2(detectionRadius, detectionRadius * __app.ellipseYMult);
		//ammo
		maxAmmo = 30;
		ammo = maxAmmo;
		//Set Color
		ammoBar.color = __app.colors[antibioticType];
    }
	
	void Update () {
		// Return if not actually placed
		if (tag == "MenuItems") {
			return;
		}
		else{
			//ammoBarWhole.SetActive(true);	
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
	
	private void updateAmmo() {
		ammo--;
        ammoBar.fillAmount = (float)ammo / (float)maxAmmo;
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

	private GameObject findBestTarget() {
		GameObject bestCandidate = null;
		bestDamage = 0f;
		GameObject[] enemies = GameObject.FindGameObjectsWithTag("Enemy");

		foreach (GameObject enemy in enemies) {
			if (checkInsideEllipse((Vector2)enemy.transform.position)) {
				Enemy enemyScript = enemy.GetComponent<Enemy>();
				float damage = baseDamages[enemyScript.species];

				// If tower can do damage to the enemy && If the enemy has not mutated against tower
				if (damage > bestDamage && !enemyScript.checkResistance(antibioticType)) { 
					bestCandidate = enemy;
					bestDamage = damage;
					if (damage == 1) { // No need to continue if we found a prime target
						break;
					}
				}
			}
		}
		if (bestCandidate != null) {
			// print(antibioticType + " targeting: " + bestCandidate.GetComponent<Enemy>().species + " Best damage: " + bestDamage);
		}
		return bestCandidate;
	}

	private bool checkInsideEllipse(Vector2 enemyPos) {
		float run = enemyPos.x - transform.position.x;
		float rise = enemyPos.y - transform.position.y + __app.towerShadowYOffset;
		float enemyDist = Mathf.Sqrt(Mathf.Pow(run, 2f) + Mathf.Pow(rise, 2f));

		float angle = Mathf.Atan2(rise, run);
		float ellipseDist = Mathf.Sqrt(Mathf.Pow((Mathf.Cos(angle) * ellipseDetectionRadius.x), 2f) + 
						               Mathf.Pow((Mathf.Sin(angle) * ellipseDetectionRadius.y + __app.towerShadowYOffset), 2f));

		return enemyDist <= ellipseDist;
	}
	
	private void activate() {
		coolingDown = true;
		cdTime = coolDown;
		if (ammo < 1) {
			return;
		}
		if (type == 0 || type == 1) { 
			
			// If there's no target or if the target could be better, re-search.
			if (target == null || bestDamage != 1f) {
				target = findBestTarget();
			}
			if (target != null) { 
				// Check if target is out of bounds
				if (!checkInsideEllipse((Vector2)(target.transform.position))) {
					decoupleTarget();
				}
				else {
					if (type == 0) { // PELLET
						shoot(target);
						updateAmmo();
					}
					else if(type == 1) { // LASER
						fireLaser(target);
						updateAmmo();
					}
				}
			}
			// If ammo gone
			if (ammo < 1) {
				//Destroy(gameObject);
				Color tmp = GetComponent<SpriteRenderer>().color;
				tmp.a = 0.5f;
				GetComponent<SpriteRenderer>().color = tmp;
			}
			else {
				//Change opacity to match tower
				Color tmp = GetComponent<SpriteRenderer>().color;
				tmp.a = 1f;
				GetComponent<SpriteRenderer>().color = tmp;
			}

		}
		else if (type == 2) { // AOE
			explode();
		}
	}

	private void explode() {
		List<Enemy> targets = findAllTargets();
		bool boom = false;

		foreach (Enemy t in targets) {
			t.hurt(5, this);
			boom = true;
		}
		if (boom) {
			// Visual
			GameObject myAOE = Instantiate(bombAOE);
			myAOE.GetComponent<SpriteRenderer>().color = (Vector4)__app.colors[antibioticType] + (new Vector4(0.1f, 0.1f , 0.1f, -0.5f));
			myAOE.transform.position = transform.position + new Vector3(0,__app.towerShadowYOffset,0);
			myAOE.transform.localScale += new Vector3(1,0,0);
			appScript.explode(gameObject.transform.position, 10, .1f, color);
			updateAmmo();
		}
	}

	private List<Enemy> findAllTargets() {
		GameObject[] enemies = GameObject.FindGameObjectsWithTag("Enemy");
		List<Enemy> targets = new List<Enemy>();

		foreach (GameObject enemy in enemies) {
			if (checkInsideEllipse((Vector2)enemy.transform.position)) {
				Enemy enemyScript = enemy.GetComponent<Enemy>();
				float damage = baseDamages[enemyScript.species];

				// If tower can do damage to the enemy && If the enemy has not mutated against tower
				if (damage > 0 && !enemyScript.checkResistance(antibioticType)) { 
					targets.Add(enemyScript);
				}
			}
		}
		return targets;
	}

	private void fireLaser(GameObject enemy) {
		
		// Physical
		float run = enemy.transform.position.x - transform.position.x;
		float rise = enemy.transform.position.y - transform.position.y;
		float angle = Mathf.Atan2(rise, run);
	
		Vector2 ePos = new Vector2(Mathf.Cos(angle) * ellipseDetectionRadius.x,
									 Mathf.Sin(angle) * ellipseDetectionRadius.y);
		float eX = Mathf.Cos(angle) * ellipseDetectionRadius.x;
		float eY = Mathf.Sin(angle) * ellipseDetectionRadius.y;
		float ellipseDist = Mathf.Sqrt(Mathf.Pow(eX, 2f) + 
						               Mathf.Pow(eY, 2f));
									   
		RaycastHit2D[] rc;
		rc = Physics2D.RaycastAll(transform.position, new Vector2(run, rise), ellipseDist, enemyMask);
		foreach (RaycastHit2D r in rc) {
			r.collider.gameObject.GetComponent<Enemy>().hurt(5, this);
		}

		// Visual
		firingLaser = true;
		if (!lr.enabled) {
			lr.enabled = true;
		}
		lr.startColor = color;
		lr.endColor = color;
		lr.SetPosition(1, ePos);
	}
	
	private void shoot(GameObject enemy) {
		if (type == 0) {
			GameObject myProjectile = Instantiate(projectile);
			myProjectile.transform.position = transform.position;

			float run = enemy.transform.position.x - transform.position.x;
			float rise = enemy.transform.position.y - transform.position.y;
			float distance = Mathf.Sqrt(Mathf.Pow(run, 2f) + Mathf.Pow(rise, 2f));
			float xsp = (run / distance) * projectileSpeed;
			float ysp = (rise / distance) * projectileSpeed;
			myProjectile.GetComponent<Projectile>().setVals(this, projectileSize, 
															 projectilePierce, xsp, ysp);
			//Set Color
			myProjectile.GetComponent<SpriteRenderer>().color = __app.colors[antibioticType];
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
			towerManager.drawEllipse(detectionRadius);
			towerManager.setLabels(towerName, cost);
			towerManager.enableSellButton();
		}
	}
}