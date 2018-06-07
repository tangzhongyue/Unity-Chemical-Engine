namespace UCE
{
    using UnityEngine;
    using VRTK;

    // TODO: wait a few seconds and put off fire
    public class TriangleControl : VRTK_InteractableObject
    {
        public enum ControlType : int { Temperature, Pressure };
        public enum ControlDirection : int { Up=1, Down=-1 };

        public ControlType controlType;
        public ControlDirection controlDirection;

        private TextMesh text;
        private static float temperature = 20f;
        private static float pressure = 101f;
        private bool isUsing = false;

        void Start()
        {
            if (controlType == ControlType.Temperature)
            {
                text = GameObject.Find("temp text").GetComponent<TextMesh>();
            } else
            {
                text = GameObject.Find("pressure text").GetComponent<TextMesh>();
            }
        }

        public override void StartUsing(VRTK_InteractUse usingObject)
        {
            base.StartUsing(usingObject);
            isUsing = true;
            if (controlType == ControlType.Pressure)
            {
                UCE_Pressure.StartChanging();
            }
        }

        public override void StopUsing(VRTK_InteractUse usingObject)
        {
            base.StopUsing(usingObject);
            isUsing = false;
            if (controlType == ControlType.Pressure)
            {
                UCE_Pressure.StopChanging();
            }
        }

        protected override void FixedUpdate()
        {
            base.Update();
            if (isUsing)
            {
                if (controlType == ControlType.Temperature)
                {
                    temperature += (int)controlDirection * 20 * Time.fixedDeltaTime;
                    text.text = temperature.ToString("f0") + " ℃";
                    UCE_Global.env_temperature = (int)Mathf.Round(temperature);
                }
                else
                {
                    pressure += (int)controlDirection * 100 * Time.fixedDeltaTime;
                    if (pressure < 1.01f)
                        pressure = 1f;
                    text.text = pressure.ToString() + " KPa";
                    UCE_Global.env_pressure = (int)Mathf.Round(pressure);
                }
            }
        }
    }
}