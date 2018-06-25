namespace UCE
{ 
    using UnityEngine;

    public class UCE_Heatable : MonoBehaviour {
        
        public enum Type
        {
            Normal,
            Burner,
        }

        public Type type;
        public float boilingPoint = 100;

        [HideInInspector]
        public float temperature = 20;
        [HideInInspector]
        public Vector3 position;
        [HideInInspector]
        public bool isHeating = false;

        private UCE_Heatable childHeatable;

        private static float heatConst = 10f;
        private static float coolConst = 10f;

        public void SetFire()
        {
            isHeating = true;
            if (childHeatable)
            {
                childHeatable.isHeating = true;
            }
        }

        public void PutOutFire()
        {
            isHeating = false;
            if (childHeatable)
            {
                childHeatable.isHeating = false;
            }
        }

        void OnTriggerEnter(Collider other)
        {
            if (type == Type.Burner)
            {
                UCE_Heatable heatable = other.GetComponent<UCE_Heatable>();
                if (heatable)
                {
                    // This is required because the following corner case can happen:
                    // Enter -> Enter -> Exit -> Exit
                    if (childHeatable)
                    {
                        childHeatable.isHeating = false;
                        childHeatable = null;
                    }
                    childHeatable = heatable;
                    if (isHeating == true)
                    {
                        heatable.isHeating = false;
                    }
                }
            }
        }

        void OnTriggerExit(Collider other)
        {
            if (type == Type.Burner)
            {
                UCE_Heatable heatable = other.GetComponent<UCE_Heatable>();
                if (heatable)
                {
                    heatable.isHeating = false;
                    childHeatable = null;
                }
            }
        }

        void Start()
        {
            temperature = UCE_Global.env_temperature;
            if (type == Type.Burner)
            {
                position = gameObject.transform.position;
                UCE_Global.Register(this);
            }
        }

        void Update()
        {
            if (type == Type.Normal)
            {
                if (isHeating)
                {
                    if (temperature < boilingPoint)
                    {
                        temperature += Time.deltaTime * heatConst;
                    }
                }
                else if (temperature < UCE_Global.env_temperature - 0.01f)
                {
                    temperature = UCE_Global.env_temperature;
                }
                else if (temperature > UCE_Global.env_temperature + 0.01f)
                {
                    temperature -= Time.deltaTime * coolConst;
                }
            }
        }
    }
}