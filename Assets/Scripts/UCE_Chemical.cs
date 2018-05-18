namespace UCE
{
    using UnityEngine;
    using System.Collections.Generic;
    public class UCE_Chemical : MonoBehaviour
    {
        public UCE_Engine.ChemicalAmount chemicalAmount;

        void OnTriggerEnter(Collider other)
        {
            UCE_Chemical otherChemical = other.GetComponent<UCE_Chemical>();

            // if two chemical collide
            if (otherChemical)
            {
                List<UCE_Engine.ChemicalAmount> reactants = new List<UCE_Engine.ChemicalAmount>();
                reactants.Add(chemicalAmount);
                reactants.Add(otherChemical.chemicalAmount);

                // invoke ChemicalEngine to get reaction result
                UCE_Animation.Animation result = UCE_Engine.React(reactants);

                // use animation to perform result
                UCE_Animation.Perform(gameObject, result);
            }
        }
    }
}