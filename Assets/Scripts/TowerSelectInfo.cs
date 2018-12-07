using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class TowerSelectInfo : MonoBehaviour {


	private TowerManager towerManager;
	// Use this for initialization
	void Start () {
	towerManager = GameObject.Find("Game").GetComponent<TowerManager>();
	}
	
	// Update is called once per frame
	void Update () {
			if(towerManager.SelectedTower && towerManager.SelectedTower.tag == "Tower"){
			transform.position += new Vector3((-7f - transform.position.x) * 0.3f,0f,0f);
		}
		else{
			transform.position += new Vector3((-12f - transform.position.x)*0.1f, 0f,0f);
		}
		
		if(transform.position.x < -10 && towerManager.SelectedTower == null){
			gameObject.SetActive(false);
		}
	}
}
