using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GasSimulator : MonoBehaviour {

	float volume = 0.0f;
	// Use this for initialization
	private Obi.ObiEmitter emitter;
	// Use this for initialization
	void Start()
	{
		emitter = this.GetComponent<Obi.ObiEmitter>();
	}

	// Update is called once per frame
	void Update()
	{

	}

	public void MoveEimtter(Vector3 position)
	{
		emitter.transform.position = position;
	}

	public void Emit(float amount)
	{
		emitter.speed = amount * 100;
	}

	public void StopEmitting()
	{
		emitter.speed = 0;
	}
}
