using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class __app : MonoBehaviour {

	public float baseMutationChance = 0.1f;

	private float strepMutationChance = 0.1f;
	private float pneuMutationChance = 0.1f;
	private float staphMutationChance = 0.1f;
	private float TBMutationChance = 0.1f;
	private IDictionary<string, float> mutationChances = new Dictionary<string, float>() {
											{"staph", 1f},
											{"strep", 1f},
											{"pneu", 1f},
											{"TB", 1f}
	};


	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		
	}

	public void changeMutationChance(string species, float percentIncrease) {

	}
}
