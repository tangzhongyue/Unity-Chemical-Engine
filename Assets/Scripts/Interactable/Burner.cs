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
            GetComponent<Fire>().SetFire();
            transform.Find("Flames").GetComponent<UCE_Heatable>().SetFire();
            onFire = true;
            TipBoard.Progress(0, 1);
            TipBoard.Progress(5, 1);
        }

        public void PutOutFire()
        {
			GetComponent<Fire>().PutOutFire();
            transform.Find("Flames").GetComponent<UCE_Heatable>().PutOutFire();
            onFire = false;
        }
    }
}