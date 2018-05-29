using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Xml;
using System.IO;
using System.Collections.Specialized;


public class substanceInfo
{
    public double amount;   //mol
    public substanceType type;
    public double amount2;
    public substanceType type2;

    public substanceInfo()
    {
        amount = 0;
        type = substanceType.None;
        amount2 = 0;
        type2 = substanceType.None;
    }
}

public class ReactionSystem : MonoBehaviour
{
    public string source;
    public float sourceAmount;
    public substanceType sourceType;

    private HashSet<string> reactionsNames;
    private Dictionary<string, substanceInfo> substance;
    private XmlDocument xml;

    bool reacting;
    // Use this for initialization
    void Start()
    {
        reactionsNames = new HashSet<string>();
        substance = new Dictionary<string, substanceInfo>();

        reacting = false;

        xml = new XmlDocument();
        //Debug.Log(Application.dataPath + "/Chemistry/substances.xml");
        xml.Load(Application.dataPath + "/Chemistry/substances.xml");
        //Debug.Log(xml.GetElementsByTagName("Np").ToString());

        substance.Add("Hp", new substanceInfo());
        substance["Hp"].amount = sourceAmount;
        substance["Hp"].type = sourceType;
    }

    // Update is called once per frame
    void Update()
    {
        if (reactionsNames.Count == 0)
            return;
        ArrayList endReaction = new ArrayList();
        foreach(string rTag in reactionsNames)
        {
            XmlNodeList reactants = xml.SelectNodes("root/reaction/" + rTag + "/" + "reactant");
            foreach(XmlElement reactant in reactants)
            {
                if(substance[reactant.InnerText].amount < 0.1)
                {
                    endReaction.Add(rTag);
                    break;
                }
                substance[reactant.InnerText].amount -= 0.1 * double.Parse(reactant.GetAttribute("rate"));
                Debug.Log(substance[reactant.InnerText].amount);
            }
            XmlNodeList products = xml.SelectNodes("root/reaction/" + rTag + "/" + "product");
            foreach(XmlElement product in products)
            {
                substance[product.InnerText].amount += 0.1 * double.Parse(product.GetAttribute("rate"));
            }
        }
        foreach (string tmpR in endReaction)
            reactionsNames.Remove(tmpR);
    }

    public void AddReactant(GameObject obj)
    {
        Substance[] subs = obj.GetComponents<Substance>();
        foreach(Substance sub in subs)
        {
            if (!substance.ContainsKey(sub.name))
            {
                substance.Add(sub.name, new substanceInfo());
                substance[sub.name].type = sub.type;
                substance[sub.name].amount = sub.amount;
            }
            else if (substance[sub.name].type == sub.type)
            {
                substance[sub.name].amount += sub.amount;
            }
            else if(substance[sub.name].type2 == sub.type)
            {
                substance[sub.name].amount2 += sub.amount;
            }
            else
            {
                Debug.LogError("wrong substance type");
                return;
            }
            Destroy(sub);
        }
        
        foreach(string subName in substance.Keys)
        {
            //Debug.Log(subName);
            //Debug.Log(xml.SelectSingleNode(subName))
            //XmlNode subXml = xml.SelectSingleNode(subName);
            //XmlNodeList reactionTags = subXml.SelectNodes("reactionTags");
            XmlNodeList reactionTags = xml.SelectNodes("root/substance/"+subName+"/"+ "reactionTag");
            foreach (XmlElement rtag in reactionTags)
            {
                string reactionTag = rtag.InnerText;
                if (reactionsNames.Contains(reactionTag))
                {
                    continue;
                }
                XmlNodeList reactants = xml.SelectNodes("root/reaction/" + reactionTag + "/" + "reactant");
                bool canReact = true;
                foreach(XmlElement rtant in reactants)
                {
                    if (substance[rtant.InnerText].amount == 0) {
                        canReact = false;
                        break;
                    }
                }
                if (canReact)
                {
                    reactionsNames.Add(reactionTag);
                    XmlNodeList products = xml.SelectNodes("root/reaction/" + reactionTag + "/" + "product");
                    foreach (XmlElement product in products)
                    {
                        if (!substance.ContainsKey(product.InnerText))
                        {
                            substance.Add(product.InnerText, new substanceInfo());
                            substance[product.InnerText].type = (substanceType)System.Enum.Parse(typeof(substanceType), product.GetAttribute("type"), true);
                            substance[product.InnerText].amount = 0;
                        }
                    }
                }
            }
        }
    }
 
}
