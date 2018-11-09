using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Particle : MonoBehaviour {
	private Vector3 velocity;
	// Use this for initialization
	void Start () {
		
	}
	
	public void setVelocity(float vel){
		velocity = new Vector3(Random.Range(-vel, vel),Random.Range(-vel, vel), 0f);
	}
	
	// Update is called once per frame
	void Update () {
		transform.position += velocity;
		velocity.y-=0.01f;
		Destroy(gameObject, 1);
	}
}
