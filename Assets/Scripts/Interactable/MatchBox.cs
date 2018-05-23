namespace UCE
{
    using UnityEngine;

    public class MatchBox : MonoBehaviour
    {
        public float sensitivity = 0.4f;

        private float enterTime;
        private Vector3 enterPos;

        void OnTriggerEnter(Collider other)
        {
            Match match = other.GetComponent<Match>();
            if (match)
            {
                enterTime = Time.time;
                enterPos = other.transform.position;
            }
        }

        void OnTriggerExit(Collider other)
        {
            Match match = other.GetComponent<Match>();
            if (match)
            {
                float dx = (other.transform.position - enterPos).magnitude;
                float dt = Time.time - enterTime;
                if (dx / dt > sensitivity)
                {
                    match.SetFire();
                }
            }
        }
    }
}