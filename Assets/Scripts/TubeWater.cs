namespace UCE
{
    using UnityEngine;

    public class TubeWater : MonoBehaviour
    {
        public UCE_Engine.ChemicalType type;

        void OnTriggerEnter(Collider other)
        {
            Dropper dropper = other.GetComponent<Dropper>();
            if (dropper)
            {
                dropper.inTube = true;
                dropper.type = type;
                dropper.material = GetComponent<Renderer>().material;
            }
        }

        void OnTriggerExit(Collider other)
        {
            Dropper dropper = other.GetComponent<Dropper>();
            if (dropper)
            {
                dropper.inTube = false;
            }
        }
    }
}