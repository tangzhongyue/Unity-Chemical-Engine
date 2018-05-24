namespace UCE
{
    using System.Collections;
    using System.Collections.Generic;
    using UnityEngine;

    public class Beaker : MonoBehaviour
    {

        void OnTriggerEnter(Collider other)
        {
            Dropper dropper = other.GetComponent<Dropper>();
            if (dropper)
            {
                dropper.inBeaker = true;
                dropper.beakerWaterGo = transform.Find("water").gameObject;
            }
        }

        void OnTriggerExit(Collider other)
        {
            Dropper dropper = other.GetComponent<Dropper>();
            if (dropper)
            {
                dropper.inBeaker = false;
            }
        }
    }
}