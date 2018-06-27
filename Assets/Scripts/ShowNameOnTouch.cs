namespace UCE
{
    using UnityEngine;
    using System.Collections.Generic;

    public class ShowNameOnTouch : MonoBehaviour {

        public string showName;
        public Vector3 offset;

        private bool show = false;
        private GameObject myObject;

        private static List<GameObject> textObjects = new List<GameObject>();

        public static GameObject AllocateText()
        {
            GameObject ret = null;
            for (int i = 0; i < textObjects.Count; i++)
            {
                if (!textObjects[i].activeInHierarchy)
                {
                    ret = textObjects[i];
                    ret.SetActive(true);
                    break;
                }
            }

            if (ret == null)
            {
                ret = Instantiate<GameObject>(textObjects[0]);
                textObjects.Add(ret);
            }

            return ret;
        }

        public static void FreeText(GameObject go)
        {
            if (go)
            {
                go.SetActive(false);
                go = null;
            }
        }

        public static void SetText(GameObject go, string text)
        {
            go.transform.Find("text").GetComponent<TextMesh>().text = text;
        }

        public void ChangeName(string newName)
        {
            showName = newName;
            if (myObject)
            {
                SetText(myObject, showName);
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
                myObject = AllocateText();
                // FINAL-TODO
                SetText(myObject, showName + "(1mol)");
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
                FreeText(myObject);
            }
        }

        void OnDestroy()
        {
            FreeText(myObject);
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