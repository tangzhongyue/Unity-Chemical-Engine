using UnityEngine;
using System;
using System.Collections;
using System.Collections.Generic;


namespace Obi{

	[ExecuteInEditMode]
	public abstract class ObiEmitterShape : MonoBehaviour
	{
		[Serializable]
		public struct DistributionPoint{
			public Vector3 position;
			public Vector3 velocity;
			public Color color;

			public DistributionPoint(Vector3 position, Vector3 velocity){
				this.position = position;
				this.velocity = velocity;
				this.color = Color.white;
			}

			public DistributionPoint(Vector3 position, Vector3 velocity, Color color){
				this.position = position;
				this.velocity = velocity;
				this.color = color;
			}

			public DistributionPoint GetTransformed(Transform transform, Color multiplyColor){
				return new DistributionPoint(transform.TransformPoint(position),
											 transform.TransformVector(velocity),
										     color*multiplyColor);
			}
		}

		[SerializeProperty("Emitter")]
		[SerializeField] protected ObiEmitter emitter;

		public Color color = Color.white;

		[HideInInspector] public float particleSize = 0;
		[HideInInspector] public List<DistributionPoint> distribution = new List<DistributionPoint>();

		public ObiEmitter Emitter{
			set{
				if (emitter != value){

					if (emitter != null){
						emitter.RemoveShape(this);
					}

					emitter = value;
					
					if (emitter != null){
						emitter.AddShape(this);
					}
				}
			}
			get{return emitter;}
		}

		public void OnEnable(){
			if (emitter != null)
				emitter.AddShape(this);
		}

		public void OnDisable(){
			if (emitter != null)
				emitter.RemoveShape(this);
		}

		public abstract void GenerateDistribution();
		
	}
}

