namespace UCE
{
    using UnityEngine;

    public class Faker : MonoBehaviour
    {
        UCE_Heatable heatable;
        AirTransmit airTransmit;
        // Use this for initialization
        void Start()
        {
            heatable = transform.GetComponent<UCE_Heatable>();
            airTransmit = transform.GetComponent<AirTransmit>();
        }

        // Update is called once per frame
        void Update()
        {
            if (heatable.isHeating)
            {
                Debug.Log("Faker heating");
                airTransmit.GenerateAir(0.3f);
            }
        }
    }
}