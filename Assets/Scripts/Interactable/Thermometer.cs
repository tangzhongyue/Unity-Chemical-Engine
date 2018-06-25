namespace UCE
{
    using UnityEngine;
    using VRTK;

    public class Thermometer : MonoBehaviour
    {

        [HideInInspector]
        public static Vector3 offsetFromCenter = new Vector3(0.005f, 0.026f, -0.0965f);
        
        private GameObject myObject;
        private bool show = false;
        private int cnt = 0;
        private UCE_Heatable insideHeatable = null;

        void UpdateTemperature()
        {
            string tempStr;
            if (insideHeatable)
            {
                tempStr = "[" + insideHeatable.temperature.ToString("f1") + "]";
            }
            else
            {
                tempStr = UCE_Global.GetTemperature(transform.position).ToString("f1");
            }
            ShowNameOnTouch.SetText(myObject, tempStr + "℃");
        }

        void OnTriggerEnter(Collider other)
        {
            // Controller for Simulator, Side for HTC Vive
            if (other.name.Contains("Controller") || other.name.Contains("Side"))
            {
                cnt++;
                // When using dropper, this function might be triggered twice in a row,
                // so we need this to ensure the name text before can disapper properly
                if (show)
                {
                    return;
                }
                myObject = ShowNameOnTouch.AllocateText();
                show = true;
            }
            else
            {
                UCE_Heatable heatable = other.GetComponent<UCE_Heatable>();
                if (heatable && heatable.type == UCE_Heatable.Type.Normal)
                {
                    insideHeatable = heatable;
                }
            }
        }

        void OnTriggerExit(Collider other)
        {
            // Controller for Simulator, Side for HTC Vive
            if (myObject == null)
            {
                return;
            }
            if (other.name.Contains("Controller") || other.name.Contains("Side"))
            {
                cnt--;
                if (cnt <= 0)
                {
                    show = false;
                    ShowNameOnTouch.FreeText(myObject);
                }
            }
            else
            {
                UCE_Heatable heatable = other.GetComponent<UCE_Heatable>();
                if (heatable && heatable.type == UCE_Heatable.Type.Normal)
                {
                    insideHeatable = null;
                }
            }
        }

        // Use this for initialization
        void Start()
        {
        }

        // Update is called once per frame
        void LateUpdate()
        {
            if (show)
            {
                UpdateTemperature();
                myObject.transform.position = transform.position;// + offset;
                myObject.transform.LookAt(Camera.main.transform.position, Vector3.up);
                myObject.transform.forward = Camera.main.transform.forward;
            }
        }
    }
}