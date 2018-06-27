namespace UCE
{
    using UnityEngine;

    public class Bottle : MonoBehaviour
    {
        
        [Tooltip("This variable decides how fast the water surface decrease")]
        public float collectVelocity = 0.5f;
	    public Obi.ObiEmitter emitter;
        private bool isCollecting = false;
        private float height = 1.0f;

        public void StartCollecting()
        {
            isCollecting = true;
        }

        public void StopCollecting()
        {
            isCollecting = false;
        }

        // ideal input: 0.3/s
        public void AddAir(float amount)
        {
            height -= amount * collectVelocity;
            if (height < 0f)
            {
                emitter.speed = 0.5f * collectVelocity;
                height = 0f;
                isCollecting = false;
                transform.Find("bottleWaterUp").gameObject.SetActive(false);
                TipBoard.Progress(4, 1);
            }
            transform.Find("bottleWaterUp").localScale = new Vector3(1f, height, 1f);
            TipBoard.Progress(4, 0);
        }

        // Update is called once per frame
        void Update()
        {
            if (isCollecting)
            {
                AddAir(Time.deltaTime * 0.3f);
            }
        }
    }
}
