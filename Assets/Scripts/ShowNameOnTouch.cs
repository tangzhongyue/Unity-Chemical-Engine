namespace UCE
{
    using UnityEngine;
    using VRTK;

    public class ShowNameOnTouch : MonoBehaviour {

        public string showName;
        public Vector3 offset;
        public GameObject textObject;

        private bool show = false;

	    void OnTriggerEnter(Collider other)
        {
            //if (other.GetComponent<VRTK_ControllerEvents>())
            {
                show = true;
            }
        }

        void OnTriggerExit(Collider other)
        {
            //if (other.GetComponent<VRTK_ControllerEvents>())
            {
                show = false;
            }
        }

        void OnGUI()
        {
            //if (show)
            {
                textObject.SetActive(true);
                textObject.transform.position = transform.position + offset;
                textObject.GetComponent<TextMesh>().text = showName;
                textObject.transform.LookAt(Camera.main.transform.position, Vector3.up);
                textObject.transform.forward = Camera.main.transform.forward;
            }
        }

    }
}