using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MusicPlayer : MonoBehaviour {
	private AudioClip song;
	private AudioSource source;

	// Use this for initialization
	void Start () {
		source = GetComponent<AudioSource>();
	}
	
	// Update is called once per frame
	void Update () {
		
	}
	
	public void NewSong(string name){
		song = Resources.Load("Music/"+name) as AudioClip;
		source.clip = song;
		source.Play();
		updatePlay();
	}
	
	public void updatePlay(){
		var mus = GetComponent<__app>().getMusic();
		if(mus){
			source.volume = 1;
			//GetComponent<AudioSource>().Play();
		}
		else{
			source.volume = 0;
			//GetComponent<AudioSource>().Stop();
		}
	}
}
