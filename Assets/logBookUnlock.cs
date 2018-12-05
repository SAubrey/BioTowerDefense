using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class logBookUnlock : MonoBehaviour {
	private string id;
	// Use this for initialization
	void Start () {
		id = "";
	}
	
	// Update is called once per frame
	void Update () {
		
	}
	
	public void setId(string i){
		id = i;
		Debug.Log("ID SET TO: "+id);
		__app.logbook[id] = true;
	}

	void OnMouseDown()
	{
		Destroy(gameObject);
	}
}
