using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UpsideDownCubeController : MonoBehaviour {
	public Obi.ObiEmitter emitter;
	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		if (emitter.ActiveParticles == emitter.NumParticles)
		{
			this.gameObject.SetActive(false);
		}
			
	}
}
