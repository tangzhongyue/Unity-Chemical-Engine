using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bubbles : MonoBehaviour
{
	float volume = 0.0f;
	// Use this for initialization
	void Start()
	{
	}

	// Update is called once per frame
	void Update()
	{

	}

	public void Emit(float amount) {
		volume += amount;
		Debug.Log("emit"+(int)(volume * 1000));
		GetComponent<ParticleSystem>().Emit((int)(volume * 1000));
		volume -= (float)((int)(volume * 1000)) / 1000;
	}

	public void MoveBubblesToNewPosition(Transform t) {
		float yoffset = t.position.y - transform.position.y;
		float newLifeTime = GetComponent<ParticleSystem>().main.startLifetime.constant - yoffset / GetComponent<ParticleSystem>().velocityOverLifetime.y.constantMax;
		var main = GetComponent<ParticleSystem>().main;
		main.startLifetime = newLifeTime;
		transform.position = t.position;
	}

	public void StopBubbles()
	{
		GetComponent<ParticleSystem>().Stop();
	}
}
