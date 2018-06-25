namespace UCE
{
    using System;
    using UnityEngine;

    public class UCE_Global : MonoBehaviour {

        // This is controlled by the triangle buttons
        public static int env_temperature = 20;
        public static int env_pressure = 121;

        // TODO: support multiple burner
        private static UCE_Heatable burner;
        private static Vector3 lastPos = new Vector3(0, 0, 0);
        private static float lastTemp = 0;

        public static void Register(UCE_Heatable heatable)
        {
            burner = heatable;
        }

        /// Determine the temperature based on the distance 
        /// between the position and each heat source.
        /// Note that the unit for result is ℃.
	    public static float GetTemperature(Vector3 position)
        {
            float res = env_temperature;
            if (burner.isHeating)
            {
                if (lastPos == position)
                {
                    return lastTemp;
                }
                // we should use the thermometer head to detect temperature, so 
                // we need to move the position from the center to the head
                position += Thermometer.offsetFromCenter;
                Vector3 distance3d = position - burner.position;
                distance3d = new Vector3(distance3d.x, (distance3d.y - 0.04f)/1.6f, distance3d.z);
                float magnitude = distance3d.magnitude;
                
                // inner flame
                if (magnitude < 0.012f)
                {
                    // (0, 648) -> (0.012, 600)
                    res = 648 - magnitude * 4000;
                }
                // outter flame
                else if (magnitude < 0.025f)
                {
                    // (0.012, 600) -> (0.025, 405)
                    res = 780 - magnitude * 15000;
                }
                else
                {
                    // (0.025, 405) -> (inf, env_temperature)
                    res = (405 - env_temperature) * 0.025f * 0.025f / magnitude / magnitude + env_temperature;
                }
            }

            lastPos = position;
            lastTemp = res;
            return res;
        }
    }
}