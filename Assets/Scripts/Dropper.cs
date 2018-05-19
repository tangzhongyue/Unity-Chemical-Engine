namespace UCE
{
    using UnityEngine;
    using System.Collections;
    using VRTK;

    public class Dropper : VRTK_InteractableObject
    {
        public UCE_Engine.ChemicalType type;
        public bool inTube = false;
        public Material material = null;

        private bool hasWater = false;

        public override void StartUsing(VRTK_InteractUse usingObject)
        {
            base.StartUsing(usingObject);

            if (inTube)
            {
                GameObject go = transform.Find("water").gameObject;
                go.SetActive(true);
                hasWater = true;
                go.GetComponent<Renderer>().material = material;
            }
            else if (hasWater)
            {
                GameObject go = transform.Find("water").gameObject;
                go.SetActive(false);
                hasWater = false;
            }
        }
    }
}
