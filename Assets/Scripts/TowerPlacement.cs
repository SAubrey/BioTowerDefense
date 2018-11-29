using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TowerPlacement : MonoBehaviour {

	public int hitting;

	void Start () {
	}
	
	void Update () {
	}

	void OnTriggerEnter2D(Collider2D other) {
		if (other.tag != "Tower" && other.tag !="Enemy") {
			hitting += 1;
		}
	}

	void OnTriggerExit2D(Collider2D other) {
		if (other.tag != "Tower" && other.tag !="Enemy") {
			hitting -= 1;
		}
	}
}
