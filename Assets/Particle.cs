using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Particle : MonoBehaviour {
	private Vector3 velocity;
	// Use this for initialization
	void Start () {
		velocity = new Vector3(Random.Range(-0.05f,0.05f),Random.Range(-0.05f,0.05f),0f);
	}
	
	// Update is called once per frame
	void Update () {
		transform.position += velocity;
		velocity.y-=0.01f;
	}
}
