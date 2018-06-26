using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

public class ReactionPhenomena : MonoBehaviour {
	
	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		
	}

	void DrawSolid(substanceInfo substance, float scaleRate) {
		ArrayList solids = substance.objects[(int)substanceType.Solid];
		foreach (GameObject solid in solids)
		{
			solid.transform.localScale *= scaleRate;
		}
	}

	void DrawLiquid(substanceInfo substance, float rate)
	{
		ArrayList liquids = substance.objects[(int)substanceType.Liquid];
		foreach (GameObject liquid in liquids)
		{
			if (substance.color[3] != 0)
			{
				Color c = new Color(substance.color[0], substance.color[1], substance.color[2], Mathf.Min(rate / substance.color[3], 0.6f));
				liquid.GetComponent<MeshRenderer>().material.color = c;
			}

		}
	}

	void DrawGas(substanceInfo substance, float reactionAmount) { 
		/*ArrayList gass = substance.objects[(int)substanceType.Gas];
		foreach (GameObject gas in gass)
		{
			gas.transform.localScale *= scaleRate;
			Color c = new Color(substance.color[0], substance.color[1], substance.color[2], 0.2f);
			gas.GetComponent<MeshRenderer>().material.color = c;
		}*/
		ArrayList gass = substance.objects[(int)substanceType.Gas];
		foreach (GameObject gas in gass)
		{
			GameObject gasCreater = (GameObject)substance.gasCreater[gass.IndexOf(gas)];
			if (gasCreater != null)
			{
				gas.GetComponent<Bubbles>().Emit(reactionAmount);
			}
			else{
				gas.GetComponent<Bubbles>().Emit(reactionAmount);
			}
		}
	}

	void DrawSubstance(substanceInfo substance, float reactionAmount, substanceType stype) { 
		switch (stype)
		{
			case substanceType.Solid:
				{
					float newAmount = substance.amount[(int)stype];
					float oldAmount = newAmount - reactionAmount;
					DrawSolid(substance, newAmount / oldAmount);
					break;
				}
			case substanceType.Liquid:
				{
					DrawLiquid(substance, (float)substance.concentration);
					break;
				}
			case substanceType.Gas:
				{
					//Debug.Log("bubbles" + reactionAmount);
					DrawGas(substance, reactionAmount);
					break;
				}
			default:
				break;
		}
	}

	public void DrawPhenomena(Dictionary<string, substanceInfo> substances, reactionInfo rctInfo, Dictionary<string, float> reactionAmounts) {
		foreach (substanceInfoOfReaction sir in rctInfo.reactants)
		{
			DrawSubstance(substances[sir.name], reactionAmounts[sir.name], sir.type);
		}
		foreach (substanceInfoOfReaction sir in rctInfo.products)
		{
			DrawSubstance(substances[sir.name], reactionAmounts[sir.name], sir.type);
			
		}
	}
}
