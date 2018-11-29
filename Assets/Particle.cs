using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Particle : MonoBehaviour {
	private Vector3 velocity;
	SpriteRenderer sr;
	private bool exploding = false;
	void Start () {
		sr = gameObject.GetComponent<SpriteRenderer>();
		Destroy(gameObject, 1);
	}
	
	public void setVelocity(float vel){
		velocity = new Vector3(Random.Range(-vel, vel),Random.Range(-vel, vel), 0f);
	}

	public void explode(float vel) {
		velocity = new Vector3(Random.Range(-vel, vel),
							   Random.Range(vel / 2, vel), 0f);
		exploding = true;
	}

	private void fade() {
		var c = sr.color;
		sr.color = new Color(c.r, c.g, c.b, c.a - 0.03f);
	}
	
	void Update () {
		transform.position += velocity;
		velocity.y -= 0.008f;
		if (exploding) { fade(); }
	}
}
