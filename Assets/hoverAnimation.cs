using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class hoverAnimation : MonoBehaviour {
	private Vector3 oldPos;
	// Use this for initialization
	void Start () {
		oldPos = transform.position;
	}
	
	// Update is called once per frame
	void Update () {
		Vector3 myVect = oldPos;
		myVect.y = oldPos.y + Mathf.Sin(Time.time*4)/9;
		transform.position = myVect;
	}
}
