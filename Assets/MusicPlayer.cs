using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MusicPlayer : MonoBehaviour {

	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		
	}
	
	public void NewSong(string name){
		GetComponent<AudioSource>().clip = Resources.Load("Music/"+name) as AudioClip;
		GetComponent<AudioSource>().Play();
	}
}
