using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Projectile : MonoBehaviour {
	//private string antibioticType;
	//private string type; 
	//private float speed = 0.6f;
	private float size = 0.5f;
	private int enemiesPierced = 0;
	private Tower towerScript;
	private Vector3 velocity;
	
	public virtual void Update () {
		// Move
		transform.position += velocity * Time.timeScale;
	}
	
	public void setVals(Tower pTow, float pSize, 
						int pPierce, float xsp, float ysp) {
		towerScript = pTow;
		size = pSize;
		velocity.x = xsp;
		velocity.y = ysp;
		transform.localScale  = new Vector3(size, size, size);
		var boxCol = GetComponent<BoxCollider2D>();
		boxCol.size = new Vector2(boxCol.size.x * 2, boxCol.size.y * 2);
	}
	
	void OnTriggerEnter2D(Collider2D col) {	
		var obj = col.gameObject;

		if (obj.tag == "Enemy") {
			obj.GetComponent<Enemy>().hurt(__app.baseDamage, towerScript);
			Destroy(gameObject);
		}
	}
}