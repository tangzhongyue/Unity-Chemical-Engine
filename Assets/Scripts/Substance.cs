using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum substanceType { Solid = 0, Liquid, Gas};

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

    void OnTriggerExit(Collider other)
    {
        //Debug.Log(other.gameObject.name);
        if (other.gameObject.GetComponent<ReactionSystem>() != null)
        {
            //Debug.Log("hah");
            float amount = other.gameObject.GetComponent<ReactionSystem>().RemoveReactant(gameObject);
            gameObject.GetComponent<Substance>().amount = amount;
        }
    }
}
