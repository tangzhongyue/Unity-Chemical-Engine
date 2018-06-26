namespace UCE
{
    using System.Collections;
    using System.Collections.Generic;
    using UnityEngine;

    public class TipBoard : MonoBehaviour
    {
        [System.Serializable]
        public class Step
        {
            public string title;
            public List<string> substeps;
        }

        public int frameCount = 7;
        public List<Step> steps;

        // sn: step number, ssn: substep number
        private static int sn = 2, ssn = 0;
        private static TipBoard tb;
        private static TextMesh textMesh, warningMesh;
        private static string title, content;
        private static int cnt = 0, greenmax = 0, greenptr = 0, fullmax = 0, fullptr = 0, warningLifetime = 0;

        public static bool Progress(int require_sn, int require_ssn)
        {
            if (sn != require_sn || ssn != require_ssn)
            {
                Debug.Log("Progress denied: " + sn.ToString() + " " + ssn.ToString());
                return false;
            }

            Debug.Log("Leave progress: " + sn.ToString() + " " + ssn.ToString());
            if (tb.steps[sn].substeps.Count == ssn+1)
            {
                greenmax = fullmax;
                return true;
            }
            else
            {
                ssn++;
            }
            UpdateText();
            return true;
        }

        public static void Warning(string warning)
        {
            warningMesh.gameObject.SetActive(true);
            warningMesh.text = warning;
            warningLifetime = 20;
        }

        private static void UpdateText()
        { 
            if (sn == tb.steps.Count)
            {
                title = "";
                content = "恭喜！您完成了本次实验";
                greenmax = 0;
                fullmax = content.Length;
                return;
            }

            title = (sn + 1).ToString() + ") " + tb.steps[sn].title + "\n    ";
            content = "";
            for (int i = 0; i < ssn; i++)
            {
                content += tb.steps[sn].substeps[i] + ", ";
            }
            greenmax = content.Length;
            content += tb.steps[sn].substeps[ssn];
            fullmax = content.Length;
        }

        // Use this for initialization
        void Start()
        {
            tb = this;
            textMesh = transform.Find("content").GetComponent<TextMesh>();
            warningMesh = transform.Find("warning").GetComponent<TextMesh>();
            UpdateText();
        }

        // Update is called once per frame
        void FixedUpdate()
        {
            cnt++;
            if (cnt > frameCount)
            {
                cnt = 0;
                bool greenUpdate = greenptr < greenmax,
                    fullUpdate = fullptr < fullmax;
                if (greenUpdate || fullUpdate)
                {
                    if (greenUpdate)
                        greenptr++;
                    if (fullUpdate)
                        fullptr++;

                    // update text mesh
                    textMesh.text = title + "<color=green>";
                    textMesh.text += content.Substring(0, greenptr) + "</color>";
                    textMesh.text += content.Substring(greenptr, fullptr-greenptr);
                }

                if (greenptr == fullptr)
                {
                    sn++;
                    ssn = 0;
                    greenptr = 0;
                    fullptr = 0;
                    UpdateText();
                }

                if (warningLifetime>0)
                {
                    warningLifetime--;
                    if (warningLifetime == 0)
                        warningMesh.gameObject.SetActive(false);
                }
            }
        }
    }
}
