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
	public GameObject emitterForGas;
	// Use this for initialization
	void Start()
	{
		solver = GameObject.Find("Solver").GetComponent<Obi.ObiSolver>();
		//emitter.Solver = solver;
		emitter.Solver = solver;
		emitter.enabled = true;
		emitter.Awake();
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
		if (!(color.Equals(new Color(1, 1, 1, 1)) || color.Equals(new Color(0,0,0,0))) && !gasParticle.GetComponent<ParticleSystem>().isPlaying) {
			Debug.Log("start emitting!");
			Debug.Log(color);
			gasParticle.GetComponent<ParticleSystem>().Play();
			gasParticle.GetComponent<Obi.ParticleAdvector>().enabled = true;
		}
		if (gasParticle.GetComponent<ParticleSystem>().isPlaying) {
			emitterForGas.GetComponent<Obi.ObiEmitter>().speed = amount * 200;
		}
		emitter.speed = amount * 100;
	}

	public void StopBubbles()
	{

		gasParticle.GetComponent<Obi.ParticleAdvector>().enabled = false;
		gasParticle.GetComponent<ParticleSystem>().Stop();
		gasParticle.GetComponent<ParticleSystem>().Clear();
		emitterForGas.GetComponent<Obi.ObiEmitter>().speed = 0;
		emitter.speed = 0;
	}

	public void setGasColor(Color color) 
	{
		this.color = color;
		ParticleSystem.MainModule main  = gasParticle.GetComponent<ParticleSystem>().main;
		main.startColor = color;
	}
}
