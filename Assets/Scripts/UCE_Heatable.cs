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

        private bool isHeating = false;
        public float temperature = 20;
        private UCE_Heatable childHeatable;

        private static float heatConst = 20f;
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
                    //Debug.Log(name + " Lost heatee " + other.name);
                    heatable.isHeating = false;
                    childHeatable = null;
                }
            }
        }

        void Start()
        {
            temperature = UCE_Global.env_temperature;
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
                        Debug.Log(name + " Heat Temperature: " + temperature);
                    }
                }
                else if (temperature < UCE_Global.env_temperature - 0.01f)
                {
                    temperature = UCE_Global.env_temperature;
                }
                else if (temperature > UCE_Global.env_temperature + 0.01f)
                {
                    temperature -= Time.deltaTime * coolConst;
                    Debug.Log(name + " Cool Temperature: " + temperature);
                }
            }
        }
    }
}