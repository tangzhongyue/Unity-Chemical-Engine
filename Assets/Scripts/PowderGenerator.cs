using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PowderGenerator : MonoBehaviour {

    public int genNum = 100;
	private int generatedNum = 0;
	private GameObject powder;

	// Use this for initialization
	void Start () {
		powder = (GameObject)Resources.Load ("powder_test");
        Debug.Log("generating");
	}
	
	// Update is called once per frame
	void FixedUpdate () {
        //Debug.Log("aha");
		generatedNum++;
		GameObject new_powder = Instantiate(powder);
		new_powder.transform.position = this.transform.position;
		if (generatedNum > genNum)
			Destroy (this);
	}
}
