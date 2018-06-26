using UnityEngine;
using System;
using System.Collections;
using System.Collections.Generic;

namespace Obi
{
	/**
	 * Sample script that colors fluid particles based on their vorticity (2D only)
	 */
	[RequireComponent(typeof(ObiEmitter))]
	public class ObiFluidPropertyColorizer : MonoBehaviour
	{
		ObiEmitter emitter;

		void Awake(){
			emitter = GetComponent<ObiEmitter>();
			emitter.OnAddedToSolver += Emitter_OnAddedToSolver;
			emitter.OnRemovedFromSolver += Emitter_OnRemovedFromSolver;
		}

		void Emitter_OnAddedToSolver (object sender, ObiActor.ObiActorSolverArgs e)
		{
			e.Solver.OnFluidUpdated += Solver_OnFluidUpdated;
		}

		void Emitter_OnRemovedFromSolver (object sender, ObiActor.ObiActorSolverArgs e)
		{
			e.Solver.OnFluidUpdated -= Solver_OnFluidUpdated;
		}

		public void OnEnable(){}

		void Solver_OnFluidUpdated (object sender, ObiSolver.ObiFluidEventArgs e)
		{
			if (!isActiveAndEnabled)
				return;

			for (int i = 0; i < emitter.particleIndices.Length; ++i){

				int k = emitter.particleIndices[i];
			
				float v = e.vorticities[k].z;
			
				if (v > 0){
					emitter.colors[i] = Color.Lerp(Color.white,Color.red,v);
				}else{
					emitter.colors[i] = Color.Lerp(Color.white,Color.blue,-v);
				}

			}
		}
	
	}
}

