using UnityEngine;
using System;
using System.Collections;
using System.Collections.Generic;

namespace Obi{

	/**
	 * Implements common functionality for ObiCollider and ObiCollider2D.
	 */
	public abstract class ObiColliderBase : MonoBehaviour
	{
		public static Dictionary<int,Component> idToCollider = new Dictionary<int, Component>(); /**< holds pairs of <IntanceID,Collider>. 
																									  Contacts returned by Obi will provide the instance ID of the 
																									  collider involved in the collision, use it to index this dictionary
																									  and find the actual object.*/

		[SerializeProperty("CollisionMaterial")]
		[SerializeField] private ObiCollisionMaterial material;

		public int phase = 0;
		public float thickness = 0;

		public ObiCollisionMaterial CollisionMaterial{
			set{
				material = value; 
				UpdateMaterial();
			}
			get{return material;}
		}

		public IntPtr OniCollider{
			get{return oniCollider;}
		}

		[HideInInspector][SerializeField] protected Component unityCollider;
		protected IntPtr oniCollider = IntPtr.Zero;
		protected ObiRigidbodyBase obiRigidbody;
		protected int currentLayer = -1;
		protected bool wasUnityColliderEnabled = true;
		protected float oldPhase = 0;
		protected float oldThickness = 0;

		protected HashSet<ObiSolver> solvers = new HashSet<ObiSolver>(); /**< set of solvers where this collider is present.*/
		protected ObiShapeTracker tracker;						 	     /**< tracker object used to determine when to update the collider's shape*/
		protected Oni.Collider adaptor = new Oni.Collider();			 /**< adaptor struct used to transfer collider data to the library.*/

		/**
		 * Creates an OniColliderTracker of the appropiate type.
   		 */
		protected abstract void CreateTracker();

		protected abstract bool IsUnityColliderEnabled();

		protected abstract void UpdateColliderAdaptor();

		protected void CreateRigidbody(){

			obiRigidbody = null;

			// find the first rigidbody up our hierarchy:
			Rigidbody rb = GetComponentInParent<Rigidbody>();
			Rigidbody2D rb2D = GetComponentInParent<Rigidbody2D>();
				
			// if we have an rigidbody above us, see if it has a ObiRigidbody component and add one if it doesn't:
			if (rb != null){

				obiRigidbody = rb.GetComponent<ObiRigidbody>();

				if (obiRigidbody == null)
					obiRigidbody = rb.gameObject.AddComponent<ObiRigidbody>();

				Oni.SetColliderRigidbody(oniCollider,obiRigidbody.OniRigidbody);

			}else if (rb2D != null){

				obiRigidbody = rb2D.GetComponent<ObiRigidbody2D>();

				if (obiRigidbody == null)
					obiRigidbody = rb2D.gameObject.AddComponent<ObiRigidbody2D>();

				Oni.SetColliderRigidbody(oniCollider,obiRigidbody.OniRigidbody);

			}else{
				Oni.SetColliderRigidbody(oniCollider,IntPtr.Zero);
			}

		}

		private void UpdateMaterial(){
			if (material != null)
				Oni.SetColliderMaterial(oniCollider,material.OniCollisionMaterial);
			else
				Oni.SetColliderMaterial(oniCollider,IntPtr.Zero);
		}

		private void OnTransformParentChanged(){
			CreateRigidbody();
		}

		/**
		 * Registers this collider in a given solver, if interesed in its layer.
		 */
		public void RegisterInSolver(ObiSolver solver, bool addToSolver){

			if (!solvers.Contains(solver)){

				// if the group's collisionLayers mask includes our layer:
				if (solver.collisionLayers == (solver.collisionLayers | (1 << gameObject.layer))){
	
					solvers.Add(solver);
				
					if (addToSolver)
						Oni.AddCollider(solver.OniSolver,oniCollider);
				}
			}
		}

		/**
		 * Removes this collider from a given solver.
		 */
		public void RemoveFromSolver(ObiSolver solver){
			solvers.Remove(solver);
			Oni.RemoveCollider(solver.OniSolver,oniCollider);
		}

		/**	
		 * Registers this collider in all solvers interested in its layer.
		 */
		private void FindSolvers(bool addToSolvers){

			if (gameObject.layer != currentLayer){

				currentLayer = gameObject.layer;

				// Remove from current solvers:
				foreach(ObiSolver solver in solvers){
					Oni.RemoveCollider(solver.OniSolver,oniCollider);
				}
	
				// Clear current groups list:
				solvers.Clear();
	
				// Recreate the group list:
				ObiSolver[] sceneSolvers = GameObject.FindObjectsOfType<ObiSolver>();
	
				// Find new solvers and add ouselves to them:
				foreach(ObiSolver solver in sceneSolvers){

					RegisterInSolver(solver,addToSolvers);

				}
			}
		}

		private void Update(){
			FindSolvers(true); // in case we have moved to a a different layer.
		}

		protected virtual void Awake(){

			wasUnityColliderEnabled = IsUnityColliderEnabled();

			// register the collider:
			idToCollider.Add(unityCollider.GetInstanceID(),unityCollider);

			CreateTracker();
			oniCollider = Oni.CreateCollider();

			FindSolvers(false);

			if (tracker != null)
				Oni.SetColliderShape(oniCollider,tracker.OniShape);

			// Send initial collider data:
			UpdateColliderAdaptor();
			Oni.UpdateCollider(oniCollider,ref adaptor);

			// Update collider material:
			UpdateMaterial();

			// Create rigidbody if necessary, and link ourselves to it:
			CreateRigidbody();

			// Subscribe collider callback:
			ObiSolver.OnUpdateColliders += UpdateIfNeeded;
		}
		
		private void OnDestroy()
		{

			// Unregister collider:
			if (unityCollider != null)
				idToCollider.Remove(unityCollider.GetInstanceID());

			// Unsubscribe collider callback:
			ObiSolver.OnUpdateColliders -= UpdateIfNeeded;

			// Destroy collider:
			Oni.DestroyCollider(oniCollider);
			oniCollider = IntPtr.Zero;

			// Destroy shape tracker:
			if (tracker != null){
				tracker.Destroy();
				tracker = null;
			}
		}

		public void OnEnable(){

			// Add collider to current solvers:
			foreach(ObiSolver solver in solvers)
				Oni.AddCollider(solver.OniSolver,oniCollider);

		}

		public void OnDisable(){

			// Remove collider from current solvers:
			foreach(ObiSolver solver in solvers)
				Oni.RemoveCollider(solver.OniSolver,oniCollider);

		}

		/** 
		 * Check if the collider transform or its shape have changed any relevant property, and update their Oni counterparts.
		 */
		private void UpdateIfNeeded(object sender, EventArgs e){
	
			if (unityCollider != null){

				// update the collider:
				bool unityColliderEnabled = IsUnityColliderEnabled();

				if (unityCollider.transform.hasChanged || 
				    phase != oldPhase ||
				    thickness != oldThickness || 
				    unityColliderEnabled != wasUnityColliderEnabled){
	
					unityCollider.transform.hasChanged = false;
					oldPhase = phase;
					oldThickness = thickness;
					wasUnityColliderEnabled = unityColliderEnabled;
	
					// remove the collider from all solver's spatial partitioning grid:
					foreach(ObiSolver solver in solvers)
						Oni.RemoveCollider(solver.OniSolver,oniCollider);
					
					// update the collider:
					UpdateColliderAdaptor();
					Oni.UpdateCollider(oniCollider,ref adaptor);
					
					// re-add the collider to all solver's spatial partitioning grid:
					if (unityColliderEnabled){
						foreach(ObiSolver solver in solvers)
							Oni.AddCollider(solver.OniSolver,oniCollider);
					}
					
				}
			}

			// update the shape:
			if (tracker != null)
				tracker.UpdateIfNeeded();

		}


	}
}

