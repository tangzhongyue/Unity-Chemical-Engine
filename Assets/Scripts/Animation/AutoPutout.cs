using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AutoPutout : MonoBehaviour {
	public GameObject match;
	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		
	}

	private void OnParticleSystemStopped() {
		match.GetComponent<UCE.Match>().onFire = false;
	}
}
