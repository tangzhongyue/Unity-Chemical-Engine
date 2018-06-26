namespace UCE
{
    using UnityEngine;

    public class AirTransmit : MonoBehaviour
    {
        public static void Connect(AirTransmit airFrom, AirTransmit airTo)
        {
            if (!airFrom || !airTo)
            {
                Debug.Log("AirTransmit: Connect object without script");
                return;
            }
            airFrom.next = airTo;
            airTo.last = airFrom;

            AirTransmit cur = airFrom;
            while (cur)
            {
                cur.end = airTo.end;
                cur = cur.last;
            }
        }

        public static void Disconnect(AirTransmit airFrom, AirTransmit airTo)
        {
            if (!airFrom || !airTo)
            {
                Debug.Log("AirTransmit: Connect object without script");
                return;
            }
            airFrom.last = null;
            airTo.next = null;
            airFrom.end = null;

            AirTransmit cur = airFrom.last;
            while (cur)
            {
                cur.end = airFrom;
                cur = cur.last;
            }
        }

        public AirTransmit last = null, next = null, end = null;

        // Use this for initialization
        void Start()
        {

        }

        // Update is called once per frame
        void Update()
        {

        }
    }
}