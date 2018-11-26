using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class dataMenu : MonoBehaviour {
	public GameObject graph;
	public GameObject towerGraph;
	private string current;
	private IDictionary<string, GameObject> panels;
	// Use this for initialization
	void Start () {
		current = "";
		panels = new Dictionary<string, GameObject>() { 
			{"damage", graph},
			{"tower", towerGraph}
		};
	}
	// Update is called once per frame
	void Update () {
		foreach(KeyValuePair<string,GameObject> panel in panels){
			if(current==panel.Key){
				panel.Value.transform.position += new Vector3(0f,(11f - panel.Value.transform.position.y)*0.1f,0f);
			}
			else{
				panel.Value.transform.position += new Vector3(0f,(18f - panel.Value.transform.position.y)*0.1f,0f);
			}
		}
	}
	public void showGraph(string name){
		current = name;
	}
}


