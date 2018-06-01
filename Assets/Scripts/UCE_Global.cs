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

        public static void Register(UCE_Heatable heatable)
        {
            burner = heatable;
        }

        /// Determine the temperature based on the distance 
        /// between the position and each heat source.
        /// Note that the unit for result is ℃.
	    public static float GetTemperature(Vector3 position)
        {
            float res = 0;
            Vector3 distance3d = position - burner.position;
            Vector2 distance2d = new Vector2(distance3d.x, distance3d.z);
            // inner flame
            if (distance2d.magnitude < 0.012f && Math.Abs(distance3d.y - 0.04f) < 0.07f)
            {
                res = 600f;
            }
            // outter flame
            else if (distance2d.magnitude < 0.025f && Math.Abs(distance3d.y - 0.05f) < 0.18f)
            {
                res = 400f;
            }
            // elsewhere
            else
            {
                res = env_temperature;
            }

            return res;
        }
    }
}