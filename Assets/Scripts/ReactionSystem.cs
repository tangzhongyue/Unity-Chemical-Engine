using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Xml;
using System.IO;
using System.Collections.Specialized;

public enum systemType { Solution, Air };

public class substanceInfo
{
	//0:solid, 1:liquid, 2:gas
	//in case that 2 states of a substance exist
	public float[] amount;   //mol
	public double concentration; //only with in solution
	public float[] color; //R:G:B:A
						  //0:solid, 1:liquid, 2:gas
	public ArrayList[] objects;
	public ArrayList gasCreater;
	public substanceInfo()
	{
		amount = new float[3];
		amount[0] = amount[1] = amount[2] = 0;
		concentration = 0;
		color = new float[4];
		objects = new ArrayList[3];
		objects[0] = new ArrayList();
		objects[1] = new ArrayList();
		objects[2] = new ArrayList();
		gasCreater = new ArrayList();
	}
}

public class substanceInfoOfReaction
{
	public string name;
	public int rate;
	public substanceType type;

	public substanceInfoOfReaction(XmlElement xe)
	{
		//Debug.Log("im here");
		name = xe.InnerText;
		rate = int.Parse(xe.GetAttribute("rate").ToString());
		type = (substanceType)System.Enum.Parse(typeof(substanceType), xe.GetAttribute("type"), true);
	}
}

public class reactionInfo
{
	public ArrayList reactants;
	public ArrayList products;
	public float speedConstant;
	public float tempConstant;
	public int startTemperature;

	public reactionInfo(XmlElement xe)
	{
		//XmlNodeList reactantsXml = xe.GetElementsByTagName("reactant");
		//XmlNodeList productsXml = xe.GetElementsByTagName("product");
		reactants = new ArrayList();
		products = new ArrayList();
		foreach (XmlElement subXml in xe.ChildNodes)
		{
			if (subXml.Name.Equals("reactant"))
				reactants.Add(new substanceInfoOfReaction(subXml));
			else if (subXml.Name.Equals("product"))
				products.Add(new substanceInfoOfReaction(subXml));
			else if (subXml.Name.Equals("rateConstant"))
				speedConstant = float.Parse(subXml.InnerText);
			else if (subXml.Name.Equals("startTemperature"))
				startTemperature = int.Parse(subXml.InnerText);
			else if (subXml.Name.Equals("tempConstant"))
				tempConstant = float.Parse(subXml.InnerText);
		}
		//Debug.Log(reactants.Count);
		/*foreach (XmlElement r in reactants)
            reactants.Add(new substanceInfoOfReaction(r));
        foreach (XmlElement p in products)
            products.Add(new substanceInfoOfReaction(p));*/
	}
}

public class ReactionSystem : MonoBehaviour
{
	//the source substance added into the system  (could be nothing later but now)  [TODO]
	[SerializeField, Header("Origin Substance")]
	public bool hasSource;
	public string source;
	public double sourceAmount;
	public substanceType sourceType;
	//the condition of the system
	[SerializeField, Header("Environment Conditions")]
	public bool openAir;
	public systemType sysType;
	public float environmentTemperature = 20.0f; // degree Celsius
	public double environmentPressure = 101;  //kPa
	public GameObject DrawSystem;
	//the local condition of a system
	//now the whole system has share one local condition, which might be changed later [TODO]
	public double localTemperature = 0;
	public double localPressure = 0;
	[SerializeField, Header("System Conditions")]
	public double capacity = 1;
	[Range(0, 1)]
	public double volumn = 0.3; //L


	private Dictionary<string, reactionInfo> reactions;
	private Dictionary<string, substanceInfo> substance;
	private XmlDocument xml;
	private GameObject[] objPrefabs;

	//this bool is used to determine if there are reactions reacting in the system
	//every new reactions added into the systen will set it to 1
	//even if the reactants' amount may be 0 due to the chain reaction calcalated before reacting
	//every update will determine if there are still reactions
	private bool isReacting = false;

	// Use this for initialization
	void Start()
	{
		reactions = new Dictionary<string, reactionInfo>();
		substance = new Dictionary<string, substanceInfo>();

		xml = new XmlDocument();
		//Debug.Log(Application.dataPath + "/Chemistry/substances.xml");
		xml.Load(Application.dataPath + "/Chemistry/substances.xml");
		//Debug.Log(xml.GetElementsByTagName("Np").ToString());

		objPrefabs = new GameObject[4];
		objPrefabs[0] = (GameObject)Resources.Load("EmptySolid");
		objPrefabs[1] = (GameObject)Resources.Load("EmptyLiquid");
		objPrefabs[2] = (GameObject)Resources.Load("EmptyGas");
		//not disitinguish liquid or other source type [TODO]
		if (hasSource)
		{
			substance.Add(source, new substanceInfo());
			substance[source].amount[1] = (float)sourceAmount;
			substance[source].concentration = sourceAmount / volumn;
			substance[source].objects[1].Add(this.gameObject);
			substance[source].color = new float[] { 0, 0, 0, 0 };
		}

		if (openAir)
		{
			substance.Add("O2", new substanceInfo());
			substance["O2"].amount[2] = 10f;
			GameObject new_obj = Instantiate((GameObject)Resources.Load("Empty"));
			new_obj.transform.position = this.gameObject.transform.position + new Vector3(0, 0.2f, 0);
			substance["O2"].objects[2].Add(new_obj);
		}
	}

	// Update is called once per frame
	void FixedUpdate()
	{
		if (!isReacting)
			return;
		isReacting = false;
		environmentTemperature = this.gameObject.GetComponent<UCE.UCE_Heatable>().temperature;
		//ArrayList endReaction = new ArrayList();
		//Debug.Log(reactions.Count);
		foreach (string rTag in reactions.Keys)
		{
			reactionInfo rctInfo = reactions[rTag];
			if (environmentTemperature < rctInfo.startTemperature)
				break;
			//deterine if a single reaction can still react
			bool isSingleReactionReacting = true;
			Dictionary<string, float> reactionAmounts = new Dictionary<string, float>();
			CalculateReactionRate(rctInfo, reactionAmounts);
			foreach (substanceInfoOfReaction sir in rctInfo.reactants)
			{
				if (substance[sir.name].amount[(int)sir.type] + reactionAmounts[sir.name] <= 0)
				{
					//the reaction is not deleted after the reactant deplete due to the reversible reaction
					//endReaction.Add(rTag);
					isSingleReactionReacting = false;
					break;
				}
			}
			if (!isSingleReactionReacting) break;
			//react
			string reactInfo = rTag + " : ";
			foreach (substanceInfoOfReaction sir in rctInfo.reactants)
			{
				float reactionAmount = reactionAmounts[sir.name];
				if (openAir && sir.name.Equals("O2"))
					continue;
				substance[sir.name].amount[(int)sir.type] += reactionAmount;
				reactInfo += sir.name + "(" + substance[sir.name].amount[(int)sir.type] + " mol";
				if (sir.type == substanceType.Liquid)
				{
					substance[sir.name].concentration += reactionAmount / volumn;
					reactInfo += ", " + substance[sir.name].concentration + " mol/L";
				}
				reactInfo += ") + ";
			}
			reactInfo = reactInfo.Substring(0, reactInfo.Length - 2);
			reactInfo += "=== ";
			foreach (substanceInfoOfReaction sir in rctInfo.products)
			{
				float reactionAmount = reactionAmounts[sir.name];
				if (openAir && sir.name.Equals("O2"))
					continue;
				substance[sir.name].amount[(int)sir.type] += reactionAmount;
				reactInfo += sir.name + "(" + substance[sir.name].amount[(int)sir.type] + " mol";
				if (sir.type == substanceType.Liquid)
				{
					substance[sir.name].concentration += reactionAmount / volumn;
					reactInfo += ", " + substance[sir.name].concentration + " mol/L";
				}
				reactInfo += ") + ";
			}
			reactInfo = reactInfo.Substring(0, reactInfo.Length - 2);
			//if there are still reacting, set true
			isReacting = true;
			DrawSystem.GetComponent<ReactionPhenomena>().DrawPhenomena(substance, rctInfo, reactionAmounts);
			Debug.Log(reactInfo);
		}
		//foreach (string tmpR in endReaction)
		//    reactionsNames.Remove(tmpR);
	}

	float[] GetColorFromXml(string RGBA)
	{
		float[] res = new float[4];
		string[] rgba = RGBA.Split(':');
		res[0] = float.Parse(rgba[0]) / 255;
		res[1] = float.Parse(rgba[1]) / 255;
		res[2] = float.Parse(rgba[2]) / 255;
		res[3] = float.Parse(rgba[3]);
		return res;
	}

	GameObject CreateNewSubstance(substanceType type, Transform t, Mesh mesh, Material mat, float[] color)
	{
		GameObject new_obj = Instantiate(objPrefabs[(int)type]);
		if (t != null)
		{
			new_obj.transform.position = new Vector3(t.position.x, t.position.y, t.position.z);
			if (type == substanceType.Liquid)
			{
				new_obj.transform.rotation = t.rotation;
				new_obj.transform.localScale = t.lossyScale;
			}
		}
		if (mesh != null)
			new_obj.GetComponent<MeshFilter>().mesh = mesh;
		if (mat != null)
			new_obj.GetComponent<MeshRenderer>().material = mat;
		if (type == substanceType.Liquid)
			new_obj.GetComponent<MeshRenderer>().material.color = new Color(color[0], color[1], color[2], 0);
		return new_obj;
	}

	//determine if the reaction could happen and add the products into the system
	void AddProduct(string reactionTag)
	{
		//Debug.Log(reactionTag);
		if (!reactions.ContainsKey(reactionTag))
			reactions.Add(reactionTag, new reactionInfo((XmlElement)xml.SelectSingleNode("root/reaction/" + reactionTag)));;
		GameObject gasCreater = null;
		XmlNodeList reactants = xml.SelectNodes("root/reaction/" + reactionTag + "/" + "reactant");
		foreach (XmlElement rtant in reactants)
		{
			int type_tmp = (int)(substanceType)System.Enum.Parse(typeof(substanceType), rtant.GetAttribute("type"), true);
			Debug.Log(rtant.InnerText + " " + substance[rtant.InnerText].amount[type_tmp]);
			if (substance[rtant.InnerText].amount[type_tmp] == 0)
				return;
			if (type_tmp == (int)substanceType.Solid) {
				gasCreater = (GameObject)substance[rtant.InnerText].objects[type_tmp][substance[rtant.InnerText].objects[type_tmp].Count-1];
			}

		}
		//Debug.Log(xml.SelectNodes("root/reaction/" + reactionTag + "/reactant").Count);

		XmlNodeList products = xml.SelectNodes("root/reaction/" + reactionTag + "/" + "product");
		foreach (XmlElement product in products)
		{
			//not work if substance exists but new state created [TODO]
			int type_tmp;
			if (!substance.ContainsKey(product.InnerText))
			{
				substance.Add(product.InnerText, new substanceInfo());
				//so that the chain reaction can be added into the system
				type_tmp = (int)(substanceType)System.Enum.Parse(typeof(substanceType), product.GetAttribute("type"), true);
				if (type_tmp != (int)substanceType.Solid)
					substance[product.InnerText].color = GetColorFromXml(xml.SelectNodes("root/substance/" + product.InnerText + "/" + "color").Item(0).InnerText);
				substance[product.InnerText].amount[type_tmp] = 0.01f;

				XmlNodeList productReactionTags = xml.SelectNodes("root/substance/" + product.InnerText + "/" + "reactionTag");
				//only the new substances should be tested if there are new reactions stand by
				foreach (XmlElement prt in productReactionTags)
					AddProduct(prt.InnerText);
			}
			type_tmp = (int)(substanceType)System.Enum.Parse(typeof(substanceType), product.GetAttribute("type"), true);
			switch ((substanceType)type_tmp)
			{
				case substanceType.Solid:
					{
						GameObject new_obj = CreateNewSubstance((substanceType)type_tmp, this.gameObject.transform, null, null, substance[product.InnerText].color);
						substance[product.InnerText].objects[type_tmp].Add(new_obj);
						break;
					}
				case substanceType.Liquid:
					{
						if (substance[product.InnerText].objects[type_tmp].Count == 0) { 
							GameObject new_obj = CreateNewSubstance((substanceType)type_tmp, this.gameObject.transform, this.gameObject.GetComponent<MeshFilter>().mesh, this.gameObject.GetComponent<MeshRenderer>().material, substance[product.InnerText].color);
							substance[product.InnerText].objects[type_tmp].Add(new_obj);

						}
						break;
					}
				case substanceType.Gas:
					{
						GameObject new_obj = CreateNewSubstance((substanceType)type_tmp, this.gameObject.transform, null, null, substance[product.InnerText].color);
						substance[product.InnerText].objects[type_tmp].Add(new_obj);
						substance[product.InnerText].gasCreater.Add(gasCreater);
						break;
					}
				default:
					break;
			}
		}
		isReacting = true;
		return;
	}

	void CalculateReactionRate(reactionInfo rct, Dictionary<string, float> reactionAmounts)
	{
		float speed = rct.speedConstant * rct.tempConstant * Mathf.Exp(-1.0f / (environmentTemperature - rct.startTemperature));
		//Debug.Log(speed);
		foreach (substanceInfoOfReaction sir in rct.reactants)
		{
			speed *= Mathf.Pow(substance[sir.name].amount[(int)sir.type], sir.rate);
		}
		foreach (substanceInfoOfReaction sir in rct.reactants)
		{
			reactionAmounts[sir.name] = -speed * 0.02f * sir.rate;
		}
		foreach (substanceInfoOfReaction sir in rct.products)
		{
			reactionAmounts[sir.name] = speed * 0.02f * sir.rate;
		}
	}

	//add a reactant to the system, called by collider
	public void AddReactant(GameObject obj)
	{
		Substance[] subs = obj.GetComponents<Substance>();
		foreach (Substance sub in subs)
		{
			if (!substance.ContainsKey(sub.name))
			{
				substance.Add(sub.name, new substanceInfo());
				if (sub.type != substanceType.Solid)
					substance[sub.name].color = GetColorFromXml(xml.SelectNodes("root/substance/" + sub.name + "/" + "color").Item(0).InnerText);
				
			}
			substance[sub.name].objects[(int)sub.type].Add(obj);
			substance[sub.name].amount[(int)sub.type] += (float)sub.amount;
			if (sub.type == substanceType.Liquid)
				substance[sub.name].concentration += sub.amount / volumn;
			Destroy(sub);
		}

		//only the new substances should be tested if there are new reactions stand by
		foreach (Substance sub in subs)
		{
			//Debug.Log(subName);
			//Debug.Log(xml.SelectSingleNode(subName))
			//XmlNode subXml = xml.SelectSingleNode(subName);
			//XmlNodeList reactionTags = subXml.SelectNodes("reactionTags");
			string subName = sub.name;
			XmlNodeList reactionTags = xml.SelectNodes("root/substance/" + subName + "/" + "reactionTag");
			//tranverse all the reactions whose reactants include the new substance
			foreach (XmlElement rtag in reactionTags)
			{
				string reactionTag = rtag.InnerText;
				//determine if the reaction could happen and add the products into the system
				AddProduct(reactionTag);
			}
		}
	}

	//return the system's reaction system information
	public Dictionary<string, substanceInfo> GetSubstances()
	{
		return substance;
	}

}
