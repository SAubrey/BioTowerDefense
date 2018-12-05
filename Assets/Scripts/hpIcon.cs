using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class hpIcon : MonoBehaviour {

	// Use this for initialization
	void Start () {
		GameObject.Find("HP Image").GetComponent<Image>().sprite = gameObject.GetComponent<SpriteRenderer>().sprite;
		Destroy(this);
	}
}
