﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum substanceType { Solid, Liquid, Gas, None };

public class Substance : MonoBehaviour {
    public string name;
    public double amount; //mol
    public substanceType type;
    // Use this for initialization
    void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		
	}

    void OnTriggerEnter(Collider other)
    {
        //Debug.Log(other.gameObject.name);
        if(other.gameObject.GetComponent<ReactionSystem>() != null)
        {
            //Debug.Log("hah");
            other.gameObject.GetComponent<ReactionSystem>().AddReactant(gameObject);
        }
    }
}