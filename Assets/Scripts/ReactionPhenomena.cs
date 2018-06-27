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
			solid.GetComponent<MeshRenderer>().material.color = new Color(substance.color[0], substance.color[1], substance.color[2], 1);
			Debug.Log((float)substance.color[0] + " " + substance.color[1] + " " + substance.color[2] + " ");
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
				Debug.Log("gascolor"+substance.color[0] + "  " + substance.color[1] + " " + substance.color[2]);
				gas.GetComponent<Bubbles>().setGasColor(new Color(substance.color[0], substance.color[1], substance.color[2], 1));

				gas.GetComponent<Bubbles>().MoveEimtter(gasCreater.transform.position);
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
					//DrawLiquid(substance, (float)substance.concentration);
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
		
		ArrayList liquids = new ArrayList();
		foreach (substanceInfoOfReaction sir in rctInfo.reactants)
		{
			Debug.Log(sir.name);

			if (sir.type == substanceType.Liquid) {
				liquids.Add(sir);
			}
			else DrawSubstance(substances[sir.name], reactionAmounts[sir.name], sir.type);
		}
		foreach (substanceInfoOfReaction sir in rctInfo.products)
		{
			Debug.Log(sir.name);

			if (sir.type == substanceType.Liquid)
			{
				liquids.Add(sir);
			}
			else DrawSubstance(substances[sir.name], reactionAmounts[sir.name], sir.type);
			
		}
		//DrawLiquids
		ArrayList fluids = new ArrayList();
		ArrayList colors = new ArrayList();
		ArrayList amounts = new ArrayList();
		foreach (substanceInfoOfReaction sir in liquids)
		{
			substanceInfo substance = substances[sir.name];
			ArrayList liqs = substance.objects[(int)substanceType.Liquid];
			foreach (GameObject liquid in liqs)
			{
				if (liquid.GetComponent<LiquidSimulator>().fluid == null)
				{
					;
				}
				if (!fluids.Contains(liquid.GetComponent<LiquidSimulator>().fluid))
				{
					Debug.Log(sir.name + " " + fluids.Count);
					fluids.Add(liquid.GetComponent<LiquidSimulator>().fluid);
					amounts.Add((float)0);
					colors.Add(Color.white);
				}
				int i = fluids.IndexOf(liquid.GetComponent<LiquidSimulator>().fluid);
				float a = (float)amounts[i];
				float sa = (float)substance.amount[(int)substanceType.Liquid];
				Color c = (Color)colors[i];
				//Debug.Log(" a" + a + " " + c.r + " " + c.g + " " + c.b + " ");
				//Debug.Log(" sa" + sa + " " + (float)substance.color[0] + " " + substance.color[1] + " " + substance.color[2] + " ");
				amounts[i] = a + sa;
				colors[i] = new Color((float)(a * c.r + sa * substance.color[0]) / (a + sa), (float)(a * c.g + sa * substance.color[1]) / (a + sa), (float)(a * c.b + sa * substance.color[2]) / (a + sa), 1.0f);
			}
		}
		foreach (GameObject fluid in fluids)
		{
			Color c = (Color)colors[fluids.IndexOf(fluid)];
			//Debug.Log(c.r + " " + c.g + " " + c.b + " ");
			fluid.GetComponent<Obi.ObiParticleRenderer>().particleColor = (Color)colors[fluids.IndexOf(fluid)];
		}
	}


}
