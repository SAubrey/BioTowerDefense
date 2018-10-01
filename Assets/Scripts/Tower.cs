using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Tower : MonoBehaviour {
	bool cd = false;
	float cdTime = 0f;
	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		if(!cd){
			var objects = GameObject.FindGameObjectsWithTag("Enemy");
			var objectCount = objects.Length;
			//Debug.Log("Object Count: "+objectCount);
			foreach (var obj in objects) {
				if(Mathf.Abs(obj.transform.position.x - transform.position.x) < 10f){
					//transform.Translate(1f,0f,0f);
					obj.GetComponent<Enemy>().Hurt(1);
					cd = true;
					cdTime = 60f;
				}
			}
		}
		else{
			if(cdTime<=0f){
				cd = false;
			}
			cdTime-=1f;
		}
	}
}
