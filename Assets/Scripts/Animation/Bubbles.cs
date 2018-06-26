using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bubbles : MonoBehaviour
{
	float volume = 0.0f;
	public Color color;
	// Use this for initialization
	public Obi.ObiEmitter emitter;
	public Obi.ObiSolver solver;
	public GameObject gasParticle;
	void Start()
	{
		
		solver = GameObject.Find("Solver").GetComponent<Obi.ObiSolver>();

		//emitter.Solver = solver;
		emitter.Solver = solver;
		emitter.enabled = true;
		emitter.Awake();
		gasParticle.GetComponent<Obi.ParticleAdvector>().solver = solver;
		solver.maxParticles += emitter.NumParticles;
	}

	// Update is called once per frame
	void Update()
	{

	}

	public void Emit(float amount) {
		if(!color.Equals(new Color(0,0,0,0)))
		   gasParticle.GetComponent<ParticleSystem>().Play();
		emitter.speed = amount * 100;
	}

	public void StopBubbles()
	{
		gasParticle.GetComponent<ParticleSystem>().Stop();
		emitter.speed = 0;
	}
}
