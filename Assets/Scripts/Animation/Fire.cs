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
		//Debug.Log("set");
		foreach (GameObject p in Flames) {
			p.GetComponent<ParticleSystem>().Play();
		}
	}

	public void PutOutFire()
	{
		//Debug.Log("out");
		foreach (GameObject p in Flames)
		{
			p.GetComponent<ParticleSystem>().Clear();
			p.GetComponent<ParticleSystem>().Stop();
		}
	}
}
