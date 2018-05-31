using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

public class Bubbles : MonoBehaviour {
	private SerializedObject bubbles;

	// Use this for initialization
	void Start () {
		bubbles = new SerializedObject(GetComponent<ParticleSystem>());
	}
	
	// Update is called once per frame
	void Update () {
		
	}

	public void CreateBubbles(float r, int rateOverTime) {
		bubbles.FindProperty("ShapeModule.radius").floatValue = r;
		bubbles.FindProperty("EmissionModule.rateOverTime").intValue = rateOverTime;
		GetComponent<ParticleSystem>().Play();
	}

	public void ChangeBubblesState(float r, int rateOverTime)
	{
		bubbles.FindProperty("ShapeModule.radius").floatValue = r;
		bubbles.FindProperty("EmissionModule.rateOverTime").intValue = rateOverTime;
	}

	public void StopBubbles()
	{
		GetComponent<ParticleSystem>().Stop();
	}
}
