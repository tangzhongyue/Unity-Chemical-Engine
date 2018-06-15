namespace UCE
{
    using UnityEngine;

    public class PipeTrigger : MonoBehaviour {

        void Start()
        {
        }

        void OnTriggerEnter(Collider other)
        {
            if (other.name == "bottle")
            {
                Bottle bottle = other.GetComponent<Bottle>();
                bottle.StartCollecting();
                Debug.Log("PipeTrigger: in Bottle");
            }
        }

        void OnTriggerExit(Collider other)
        {
            if (other.name == "bottle")
            {
                Bottle bottle = other.GetComponent<Bottle>();
                bottle.StopCollecting();
                Debug.Log("PipeTrigger: out Bottle");
            }
        }
    }
}