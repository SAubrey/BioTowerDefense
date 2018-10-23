using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;


public class OptionMenuInterface : MonoBehaviour {
	public Text musicText;
	public Text sfxText;
	private GameObject app;
	private __app appScript;

	// Use this for initialization
	void Start () {
		app = GameObject.Find("__app");
		appScript = app.GetComponent<__app>();
		updateMusicText();
		updateSFXText();
	}
	
	// Update is called once per frame
	void Update () {
		
	}
	
	public void toggleMusic(){
		var mus = appScript.getMusic();
		appScript.setMusic(!mus);
		updateMusicText();
	}
	
	private void updateMusicText(){
		var mus = appScript.getMusic();
		var txt = "On";
		if(!mus) txt = "Off";//This has to be inverted because we grab it before we set it
		musicText.text = "Music: "+txt;
	}
	
	public void toggleSFX(){
		var sfx = appScript.getSFX();
		appScript.setSFX(!sfx);
		updateSFXText();
	}
	
	private void updateSFXText(){
		var sfx = appScript.getSFX();
		var txt = "On";
		if(!sfx) txt = "Off";//This has to be inverted because we grab it before we set it
		sfxText.text = "SFX: "+txt;
	}
}
