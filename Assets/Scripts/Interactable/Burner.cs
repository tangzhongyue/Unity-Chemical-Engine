namespace UCE
{
    using UnityEngine;

    // TODO: add animation and use coroutine to wait before set fire
    public class Burner : MonoBehaviour {

        [HideInInspector]
        public bool lidIsOn = true;

        private bool onFire = false;

        void OnTriggerEnter(Collider other)
        {
            if (!onFire)
            {
                if (!lidIsOn)
                {
                    Match match = other.GetComponent<Match>();
                    if (match && match.onFire)
                    {
                        SetFire();
                    }
                }
            }
        }

        public void SetFire()
        {
            //transform.Find("fire").gameObject.SetActive(true);
			GetComponent<Fire>().SetFire();
            onFire = true;
        }

        public void PutOutFire()
        {
            //transform.Find("fire").gameObject.SetActive(false);
			GetComponent<Fire>().PutOutFire();
            onFire = false;
        }
    }
}