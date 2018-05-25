namespace UCE
{
    using UnityEngine;
    using System.Collections.Generic;

    public class ShowNameOnTouch : MonoBehaviour {

        public string showName;
        public Vector3 offset;

        private bool show = false;
        private GameObject myObject;

        static List<GameObject> textObjects = new List<GameObject>();

        public void changeName(string newName)
        {
            showName = newName;
            if (myObject)
            {
                myObject.transform.Find("text").GetComponent<TextMesh>().text = showName;
            }
        }

        void Start()
        {
            if (textObjects.Count == 0)
            {
                textObjects.Add(GameObject.FindWithTag("NameText"));
                textObjects[0].SetActive(false);
            }
        }

	    void OnTriggerEnter(Collider other)
        {
            // Controller for Simulator, Side for HTC Vive
            if (other.name.Contains("Controller") || other.name.Contains("Side"))
            {
                // When using dropper, this function might be triggered twice in a row,
                // so we need this to ensure the name text before can disapper properly
                if (show)
                {
                    OnTriggerExit(other);
                }
                
                for (int i = 0; i < textObjects.Count; i++)
                {
                    if (!textObjects[i].activeInHierarchy)
                    {
                        myObject = textObjects[i];
                        myObject.SetActive(true);
                        myObject.transform.Find("text").GetComponent<TextMesh>().text = showName;
                        break;
                    }
                }

                if (myObject == null)
                {
                    myObject = Instantiate<GameObject>(textObjects[0]);
                    textObjects.Add(myObject);
                }

                show = true;
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
                show = false;
                myObject.SetActive(false);
                myObject = null;
            }
        }

        void OnDestroy()
        {
            if (myObject)
            {
                myObject.SetActive(false);
            }
        }

        void LateUpdate()
        {
            if (show)
            {
                myObject.transform.position = transform.position + offset;
                myObject.transform.LookAt(Camera.main.transform.position, Vector3.up);
                myObject.transform.forward = Camera.main.transform.forward;
            }
        }

    }
}