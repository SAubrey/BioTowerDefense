using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class towerAOE : MonoBehaviour {

	SpriteRenderer sr;
	void Start () {
		sr = gameObject.GetComponent<SpriteRenderer>();
	}
	
	void Update () {

		// Fade
		sr.color = new Color(sr.color.r, sr.color.g, sr.color.b, sr.color.a - 0.03f);
		if (sr.color.a <= 0) {
			Destroy(gameObject);
		}
	}
}
