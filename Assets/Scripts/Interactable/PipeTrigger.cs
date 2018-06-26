namespace UCE
{
    using UnityEngine;

    public class PipeTrigger : MonoBehaviour {
        
        public bool isGood = true;

        void OnTriggerEnter(Collider other)
        {
            if (isGood && other.name == "bottle")
            {
                AirTransmit thisAir = transform.GetComponent<AirTransmit>(),
                            otherAir = other.transform.GetComponent<AirTransmit>();
                AirTransmit.Connect(thisAir, otherAir);
            }
        }

        void OnTriggerExit(Collider other)
        {
            if (isGood && other.name == "bottle")
            {
                AirTransmit thisAir = transform.GetComponent<AirTransmit>(),
                            otherAir = other.transform.GetComponent<AirTransmit>();
                AirTransmit.Disconnect(thisAir, otherAir);
            }
        }
    }
}