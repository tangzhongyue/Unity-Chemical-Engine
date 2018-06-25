namespace UCE
{
    using UnityEngine;

    public class PipeTrigger : MonoBehaviour {

        public bool doPresentation = false;
        public bool isGood = true;
        [HideInInspector]
        public bool airComing = false;

        void OnTriggerEnter(Collider other)
        {
            if ((airComing || doPresentation) && isGood && other.name == "bottle")
            {
                Bottle bottle = other.GetComponent<Bottle>();
                bottle.StartCollecting();
            }
        }

        void OnTriggerExit(Collider other)
        {
            if ((airComing || doPresentation) && isGood && other.name == "bottle")
            {
                Bottle bottle = other.GetComponent<Bottle>();
                bottle.StopCollecting();
            }
        }
    }
}