using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Projectile : MonoBehaviour {
	private string antibioticType;
	private string type; 
	// PROJECTILE DOESN'T NEED TO STORE THESE VALUES IF THEY'RE STORED/USED BY TOWER (size, speed)
	private float size = 0.5f;
	private float speed = 0.6f;
	private int pierce = 1; // Number of enemies a projectile pierces
	private int enemiesPierced = 0;
	private GameObject tower;
	private Vector3 velocity;
	private List<GameObject> hurtEnemies;
	// Use this for initialization
	public virtual void Start () {
		hurtEnemies = new List<GameObject>();
	}
	
	// Update is called once per frame
	public virtual void Update () {
		//Move
		transform.position += velocity;
	}
	
	public void setVals(GameObject pTow, string abType, float pSize, 
						float pSpeed, int pPierce, float xsp, float ysp) {
		tower = pTow;
		antibioticType = abType;
		size = pSize;
		speed = pSpeed;
		pierce = pPierce;
		velocity.x = xsp;
		velocity.y = ysp;
		transform.localScale  = new Vector3(size, size, size);
		var boxCol = GetComponent<BoxCollider2D>();
		boxCol.size = new Vector2(boxCol.size.x * 2, boxCol.size.y * 2);
	}
	
	void OnTriggerEnter2D(Collider2D col) {	
		var obj = col.gameObject;
		// Debug.Log("Collision detected, hit obj of tag " + obj.tag);

		if (obj.tag == "Enemy") {
			//hurtEnemies.Add(obj);
			obj.GetComponent<Enemy>().hurt(5, antibioticType);
			//obj.GetComponent<Enemy>().speed-=2;
			enemiesPierced++;
			if (enemiesPierced >= pierce) {
				Destroy(gameObject);
			}
		}
	}
}