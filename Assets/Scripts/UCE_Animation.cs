namespace UCE
{
    using UnityEngine;
    using System.Collections;
    public class UCE_Animation : MonoBehaviour
    {
        public enum Animation : int
        {
            NoAnimation,
            // Solid
            SolidDisappear, SolidMelt,
            // Liquid
            LiquidReduce,
            // Others
            // Create new partical system at go's position
            Explode, Fog,
        }

        public static void Perform(GameObject go, Animation type)
        {
            Debug.Log(go.ToString() + type.ToString());
            switch(type)
            {
                case Animation.NoAnimation:
                    break;
                case Animation.SolidDisappear:
                    Destroy(go);
                    break;
                case Animation.SolidMelt:
                    go.transform.localScale *= 0.5f;
                    break;
                case Animation.LiquidReduce:
                    go.transform.localScale = new Vector3(go.transform.localScale.x, go.transform.localScale.y * 0.5f, go.transform.localScale.z);
                    break;
                default:
                    Debug.Log("Unrealized or Unknown Animation Type");
                    break;
            }
        }
    }
}
