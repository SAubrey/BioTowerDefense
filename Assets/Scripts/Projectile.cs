﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Projectile : MonoBehaviour {
	//private string antibioticType;
	//private string type; 
	//private float speed = 0.6f;
	private float size = 0.5f;
	private int pierce = 1; // Number of enemies a projectile pierces
	private int enemiesPierced = 0;
	private Tower towerScript;
	private Vector3 velocity;
	private List<GameObject> hurtEnemies;

	public virtual void Start () {
		hurtEnemies = new List<GameObject>();
	}
	
	public virtual void Update () {
		// Move
		transform.position += velocity * Time.timeScale;
	}
	
	public void setVals(Tower pTow, float pSize, 
						int pPierce, float xsp, float ysp) {
		towerScript = pTow;
		size = pSize;
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
			obj.GetComponent<Enemy>().hurt(5, towerScript);
			enemiesPierced++;
			if (enemiesPierced >= pierce) {
				Destroy(gameObject);
			}
		}
	}
}