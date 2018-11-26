using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class logBook : MonoBehaviour {
	//Panels
	public GameObject strep;
	public GameObject staph;
	public GameObject pneu;
	public GameObject TB;
	//Buttons
	public GameObject strepButton;
	public GameObject staphButton;
	public GameObject pneuButton;
	public GameObject TBButton;
	private string current;
	private IDictionary<string, GameObject> panels;
	private IDictionary<string, GameObject> buttons;
	// Use this for initialization
	void Start () {
		current = "";
		panels = new Dictionary<string, GameObject>() { 
			{"strep", strep},
			{"staph", staph},
			{"pneu", pneu},
			{"TB", TB}
		};
		buttons = new Dictionary<string, GameObject>() { 
			{"strep", strepButton},
			{"staph", staphButton},
			{"pneu", pneuButton},
			{"TB", TBButton}
		};
	}
	// Update is called once per frame
	void Update () {
		foreach(KeyValuePair<string,GameObject> panel in panels){
			if(current==panel.Key){
				panel.Value.transform.position += new Vector3(0f,(0f - panel.Value.transform.position.y)*0.1f,0f);
			}
			else{
				panel.Value.transform.position += new Vector3(0f,(10f - panel.Value.transform.position.y)*0.1f,0f);
			}
		}
		
		//Buttons
		foreach(KeyValuePair<string,GameObject> button in buttons){
			if(__app.logbook[button.Key]){
				button.Value.GetComponent<Image>().color = Color.white;
			}
			else{
				button.Value.GetComponent<Image>().color = Color.red;
			}
		}
	}
	
	public void showLog(string name){
		if(name!="NULL" && __app.logbook[name]){
			current = name;
		}
	}
}
