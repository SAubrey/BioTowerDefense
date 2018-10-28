using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class hoverAnimation : MonoBehaviour {
	private Vector3 oldPos;
	public float hoverSpeed = 4;
	public float hoverDistance = 9;
	public float syncMod = 0;
	// Use this for initialization
	void Start () {
		oldPos = transform.position;
	}
	
	// Update is called once per frame
	void Update () {
		Vector3 myVect = oldPos;
		myVect.y = oldPos.y + Mathf.Sin(Time.time*hoverSpeed + (GetInstanceID()*10*syncMod))/hoverDistance;
		transform.position = myVect;
	}
}
