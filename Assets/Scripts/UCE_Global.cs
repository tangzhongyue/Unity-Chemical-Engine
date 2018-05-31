namespace UCE
{
    using UnityEngine;

    public class UCE_Global : MonoBehaviour {

        // This is controlled by the triangle buttons
        public static int env_temperature = 20;
        public static int env_pressure = 121;

        /// Determine the temperature based on the distance 
        /// between the position and each heat source.
        /// Note that the unit for result is ℃.
	    public static float GetTemperature(Vector3 position)
        {
            // TODO
            float res = 0;
            return res;
        }
    }
}