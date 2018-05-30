using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ReactionPhenomena : MonoBehaviour {
	
	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		Dictionary<string, substanceInfo> substance = GetComponent< ReactSysTemplate > ().substance;
		foreach(substanceInfo si in substance.Values){ 
			
		}
	}
}
