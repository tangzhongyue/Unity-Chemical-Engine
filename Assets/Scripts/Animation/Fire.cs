using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Fire : MonoBehaviour {
	public List<GameObject> Flames;
	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		
	}

	public void SetFire()
	{
		foreach (GameObject p in Flames) {
			p.GetComponent<ParticleSystem>().Play();
		}
	}

	public void PutOutFire()
	{
		foreach (GameObject p in Flames)
		{
			p.GetComponent<ParticleSystem>().Clear();
			p.GetComponent<ParticleSystem>().Stop();
		}
	}
}
