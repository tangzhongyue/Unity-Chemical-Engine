namespace UCE
{
    using UnityEngine;

    public class PipeTrigger : MonoBehaviour {

        public GameObject bottle;

        private Bottle bot;

        void Start()
        {
            bot = bottle.GetComponent<Bottle>();
        }

        void OnTriggerEnter(Collider other)
        {
            if (other.name == "bottle")
            {
                bot.pipeIn = true;
                Debug.Log("PipeTrigger: in Bottle");
            }
        }

        void OnTriggerExit(Collider other)
        {
            if (other.name == "bottle")
            {
                bot.pipeIn = false;
                Debug.Log("PipeTrigger: out Bottle");
            }
        }
    }
}