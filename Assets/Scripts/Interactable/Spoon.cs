namespace UCE
{
    using UnityEngine;
    using VRTK;

    public class Spoon : VRTK_InteractableObject
    {
        private bool inPowder = false;
        private bool inTube = false;
        private bool hasPowder = false;
        private GameObject myPowder;
        private GameObject tubePowder;
        private GameObject faker;

        void OnTriggerEnter(Collider other)
        {
            if (other.name == "bottle powder")
            {
                inPowder = true;
            }
            else if (other.name == "tube")
            {
                inTube = true;
                tubePowder = other.transform.Find("powder").gameObject;
                faker = other.transform.Find("faker").gameObject;
            }
        }

        void OnTriggerExit(Collider other)
        {
            if (other.name == "bottle powder")
            {
                inPowder = false;
            }
            else if (other.name == "tube")
            {
                inTube = false;
                tubePowder = null;
                faker = null;
            }
        }

        public override void StartUsing(VRTK_InteractUse usingObject = null)
        {
            base.StartUsing(usingObject);
            if (inPowder)
            {
                myPowder.SetActive(true);
                hasPowder = true;
            }
            else if (inTube && hasPowder)
            {
                myPowder.SetActive(false);
                tubePowder.SetActive(true);
                faker.SetActive(false);
                hasPowder = false;
            }
        }

        // Use this for initialization
        void Start()
        {
            myPowder = transform.Find("powder").gameObject;
        }
    }
}