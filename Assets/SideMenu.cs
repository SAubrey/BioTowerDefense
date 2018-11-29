using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SideMenu : MonoBehaviour {
	private bool moving = false;
	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		if(moving){
			
		}
	}
	
	public void test(){
		moving = !moving;
	}
}