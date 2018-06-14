namespace UCE
{
    using System.Collections.Generic;
    using UnityEngine;

    public class PressureContainer : MonoBehaviour
    {
        [Tooltip("The localPosition.y of the top of the container")]
        public float topPositionY;
        [Tooltip("The localPosition.y of the bottom of the container")]
        public float bottomPositionY;
        
        private static bool isChanging = false;

        private float volume = 1f;
        private float maxVolume;
        private float originalPressure;
        private float originalHeight;
        private Color originalColor;
        private Transform lid;
        private Transform gas;
        private Material gasMat;

        public static void StartChanging()
        {
            isChanging = true;
        }

        public static void StopChanging()
        {
            isChanging = false;
        }

        void Start()
        {
            lid = transform.Find("lid");
            gas = transform.Find("gas");
            gasMat = gas.Find("gasModel").GetComponent<MeshRenderer>().material;
            originalPressure = UCE_Global.env_pressure;
            originalHeight = lid.localPosition.y - bottomPositionY;
            originalColor = gasMat.color;
            maxVolume = (topPositionY - bottomPositionY) / originalHeight;
        }

        void Update()
        {
            if (isChanging)
            {
                volume = originalPressure / (float)UCE_Global.env_pressure;
                //Debug.Log("UPressure: volume " + volume.ToString());
                UpdateModel();
            }
        }

        void UpdateModel()
        {
            float ypos = bottomPositionY + volume * originalHeight;
            if (ypos > topPositionY)
            {
                lid.localPosition = new Vector3(lid.localPosition.x, topPositionY, lid.localPosition.z);
                gas.localScale = new Vector3(1, maxVolume, 1);
            }
            else
            {
                lid.localPosition = new Vector3(lid.localPosition.x, ypos, lid.localPosition.z);
                gas.localScale = new Vector3(1, volume, 1);
            }
            gasMat.color = volume < 1f ? originalColor : originalColor / volume;
        }
    }
}