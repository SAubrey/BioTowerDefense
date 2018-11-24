using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class mutationBars : MonoBehaviour {
	
	public Image amoxBar;
	public Image methBar;
	public Image vancBar;
	public Image carbBar;
	public Image lineBar;
	public Image rifaBar;
	public Image isonBar;
	private __app appScript;
	
	

	// Use this for initialization
	void Start () {
		appScript = GameObject.Find("__app").GetComponent<__app>();
	}
	
	// Update is called once per frame
	void Update () {
		amoxBar.fillAmount = appScript.mutationChances["staph"]["amox"];
		methBar.fillAmount = appScript.mutationChances["staph"]["meth"];
		vancBar.fillAmount = appScript.mutationChances["staph"]["vanc"];
		carbBar.fillAmount = appScript.mutationChances["staph"]["carb"];
		lineBar.fillAmount = appScript.mutationChances["staph"]["line"];
		rifaBar.fillAmount = appScript.mutationChances["staph"]["rifa"];
		isonBar.fillAmount = appScript.mutationChances["staph"]["ison"];
	}
}
