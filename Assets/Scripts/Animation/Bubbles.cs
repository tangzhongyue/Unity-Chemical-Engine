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
	// Use this for initialization
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

	public void MoveEimtter(Vector3 position)
	{
		emitter.transform.position = position;
	}

	public void Emit(float amount)
	{
		if (!color.Equals(new Color(0, 0, 0, 0)) && !gasParticle.GetComponent<ParticleSystem>().isPlaying) { 
			gasParticle.GetComponent<ParticleSystem>().Play();
		gasParticle.GetComponent<Obi.ParticleAdvector>().enabled = true;
		}
			
		emitter.speed = amount * 100;
	}

	public void StopBubbles()
	{

		gasParticle.GetComponent<Obi.ParticleAdvector>().enabled = false;
		gasParticle.GetComponent<ParticleSystem>().Stop();
		gasParticle.GetComponent<ParticleSystem>().Clear();
		emitter.speed = 0;
	}
}
