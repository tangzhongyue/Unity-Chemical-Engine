using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bottle : MonoBehaviour {

    [Tooltip("This variable decides how fast the water surface decrease")]
    public float collectVelocity = 1.0f;
	public Obi.ObiEmitter emitter;
    private bool isCollecting = false;
    private float height = 1.0f;

    public void StartCollecting()
    {
        isCollecting = true;
    }

    public void StopCollecting()
    {
        isCollecting = false;
    }
	
	// Update is called once per frame
	void Update () {
		if (isCollecting)
		{
			emitter.speed = 0.5f * collectVelocity;
			height -= collectVelocity * Time.deltaTime * 0.3f;
			if (height < 0f)
			{
				height = 0f;
				isCollecting = false;
				transform.Find("bottleWaterUp").gameObject.SetActive(false);
			}
			transform.Find("bottleWaterUp").localScale = new Vector3(1f, height, 1f);
		}
		else {
			//emitter.speed = 0;
		}
	}
}
