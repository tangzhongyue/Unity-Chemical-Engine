namespace UCE
{
    using UnityEngine;
    using System.Collections;
    using VRTK;

    public class Dropper : VRTK_InteractableObject
    {
        public UCE_Engine.ChemicalType tubeType;
        public bool inTube = false;
        public Material material = null;

        public bool inBeaker = false;
        public GameObject beakerWaterGo;

        private bool hasWater = false;
        private UCE_Engine.ChemicalType type;

        public override void StartUsing(VRTK_InteractUse usingObject)
        {
            base.StartUsing(usingObject);

            if (inTube)
            {
                GameObject go = transform.Find("water").gameObject;
                go.SetActive(true);
                hasWater = true;
                go.GetComponent<Renderer>().material = material;
                type = tubeType;
            }
            else if (hasWater)
            {
                GameObject waterGo = transform.Find("water").gameObject;
                waterGo.SetActive(false);
                hasWater = false;

                if (inBeaker)
                {
                    // The beakerWaterGo has to be set false and then set true again 
                    // beacuase this is the only way to call UCE_Chemical.OnTriggerEnter() 
                    // again to check if reaction is possible now
                    beakerWaterGo.SetActive(false);

                    UCE_Chemical chemical = beakerWaterGo.GetComponent<UCE_Chemical>();
                    chemical.chemicalAmount.type = type;
                    chemical.chemicalAmount.amount = 1;
                    beakerWaterGo.GetComponent<Renderer>().material = waterGo.GetComponent<Renderer>().material;
                    beakerWaterGo.SetActive(true);
                }
            }
        }
    }
}
