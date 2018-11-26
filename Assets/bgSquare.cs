using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class bgSquare : MonoBehaviour {
	
	Vector3 spd;
	float rotSpeed;
	Vector3 rotationEuler;
	// Use this for initialization
	void Start () {	
		spd = new Vector3(0.001f*Mathf.Sign(-transform.position.x),0.001f*Mathf.Sign(-transform.position.y),0);
		float size = Random.Range(0.5f,1.5f);
		rotSpeed = Random.Range(1,2);
		transform.localScale = new Vector3(size,size,1);
		transform.position = new Vector3(Random.Range(-5,5),Random.Range(-4,4),0);
		GetComponent<SpriteRenderer>().color = new Vector4(255f,255f,255f,0.05f);
	}
	
	// Update is called once per frame
	void Update () {
		transform.position += spd;
		
		//https://answers.unity.com/questions/962329/rotating-an-image-around-z-axis-2d.html
		//user WillNode
		rotationEuler+= Vector3.forward*rotSpeed*Mathf.Sign(spd.x)*Time.deltaTime; //increment 30 degrees every second
		transform.rotation = Quaternion.Euler(rotationEuler);
	}
}
