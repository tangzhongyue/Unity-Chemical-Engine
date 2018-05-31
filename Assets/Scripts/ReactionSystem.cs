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
    public string color; //R:G:B:A
    //0:solid, 1:liquid, 2:gas
    public GameObject[] objects;

    public substanceInfo()
    {
        amount = new float[3];
        amount[0] = amount[1] = amount[2] = 0;
        concentration = 0;
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

    public reactionInfo(XmlElement xe)
    {
        //XmlNodeList reactantsXml = xe.GetElementsByTagName("reactant");
        //XmlNodeList productsXml = xe.GetElementsByTagName("product");
        reactants = new ArrayList();
        products = new ArrayList();
        foreach(XmlElement subXml in xe.ChildNodes)
        {
            if(subXml.Name.Equals("reactant"))
                reactants.Add(new substanceInfoOfReaction(subXml));
            else if(subXml.Name.Equals("product"))
                products.Add(new substanceInfoOfReaction(subXml));
            else if(subXml.Name.Equals("rateConstant"))
                speedConstant = float.Parse(subXml.InnerText);
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
    public string source;
    public double sourceAmount;
    public substanceType sourceType;
    public GameObject sourceObj;
    //the condition of the system
    [SerializeField, Header("Environment Conditions")]
    public systemType sysType;
    public double environmentTemperature = 20.0; // degree Celsius
    public double environmentPressure = 101;  //kPa
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

        objPrefabs = new GameObject[3];
        objPrefabs[0] = (GameObject)Resources.Load("EmptySolid");
        objPrefabs[1] = (GameObject)Resources.Load("EmptyLiquid");
        objPrefabs[2] = (GameObject)Resources.Load("EmptyGas");

        substance.Add("Hp", new substanceInfo());
        substance["Hp"].amount[1] = (float)sourceAmount;
        substance["Hp"].concentration = sourceAmount / volumn;
        substance["Hp"].objects[1] = sourceObj;

        substance.Add("O2", new substanceInfo());
        substance["O2"].amount[2] = 10000f;
        substance["O2"].objects[2] = Instantiate(objPrefabs[2]);
        substance["O2"].objects[2].transform.position = this.gameObject.transform.position + new Vector3(0, 0.2, 0);
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        if (!isReacting)
            return;
        isReacting = false;
        //ArrayList endReaction = new ArrayList();
        Debug.Log(reactions.Count);
        foreach (string rTag in reactions.Keys)
        {
            reactionInfo rctInfo = reactions[rTag];
            //deterine if a single reaction can still react
            bool isSingleReactionReacting = true;
            Dictionary<string, float> reactionAmounts = new Dictionary<string, float>();
            CalculateReactionRate(rctInfo, reactionAmounts);
            foreach (substanceInfoOfReaction sir in rctInfo.reactants)
            {
                if(substance[sir.name].amount[(int)sir.type] + reactionAmounts[sir.name] <= 0)
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
            Debug.Log(reactInfo);
        }
        //foreach (string tmpR in endReaction)
        //    reactionsNames.Remove(tmpR);
    }

    //determine if the reaction could happen and add the products into the system
    void AddProduct(string reactionTag)
    {
        //Debug.Log(reactionTag);
        if (reactions.ContainsKey(reactionTag))
            return;
        XmlNodeList reactants = xml.SelectNodes("root/reaction/" + reactionTag + "/" + "reactant");
        foreach (XmlElement rtant in reactants)
        {
            int type_tmp = (int)(substanceType)System.Enum.Parse(typeof(substanceType), rtant.GetAttribute("type"), true);
            Debug.Log(rtant.InnerText + " " + substance[rtant.InnerText].amount[type_tmp]);
            if (substance[rtant.InnerText].amount[type_tmp] == 0)
                return;
        }
        //Debug.Log(xml.SelectNodes("root/reaction/" + reactionTag + "/reactant").Count);
        reactions.Add(reactionTag, new reactionInfo((XmlElement)xml.SelectSingleNode("root/reaction/" + reactionTag)));
        XmlNodeList products = xml.SelectNodes("root/reaction/" + reactionTag + "/" + "product");
        foreach (XmlElement product in products)
        {
            //not work if substance exists but new state created [TODO]
            if (!substance.ContainsKey(product.InnerText))
            {
                substance.Add(product.InnerText, new substanceInfo());
                substance[product.InnerText].color = xml.SelectNodes("root/substance/" + product.InnerText + "/" + "color").ToString();
                //so that the chain reaction can be added into the system
                int type_tmp = (int)(substanceType)System.Enum.Parse(typeof(substanceType), product.GetAttribute("type"), true);
                substance[product.InnerText].amount[type_tmp] = 0.01f;
                substance[product.InnerText].objects[type_tmp] = Instantiate(objPrefabs[type_tmp]);
                substance[product.InnerText].objects[type_tmp].transform.position = this.gameObject.transform.position;
                XmlNodeList productReactionTags = xml.SelectNodes("root/substance/" + product.InnerText + "/" + "reactionTag");
                //only the new substances should be tested if there are new reactions stand by
                foreach (XmlElement prt in productReactionTags)
                    AddProduct(prt.InnerText);
            }
        }
        isReacting = true;
        return;
    }

    void CalculateReactionRate(reactionInfo rct, Dictionary<string, float> reactionAmounts)
    {
        float speed = rct.speedConstant;
        Debug.Log(speed);
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
                substance[sub.name].color = xml.SelectNodes("root/substance/" + sub.name + "/" + "color").ToString();
                substance[sub.name].objects[(int)sub.type] = obj;
            }
            substance[sub.name].amount[(int)sub.type] = (float)sub.amount;
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
