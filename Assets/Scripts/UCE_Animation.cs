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
			LiquidChangeColor,
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
					SolidMelt(go, 0.8f);
                    break;
                case Animation.LiquidReduce:
					LiquidReduce(go, 0.5f);
					break;
				case Animation.LiquidChangeColor:
					LiquidChangeColor(go, new Color(99f/255, 110f / 255, 3f / 255, 141f/255));
					break;
                default:
                    Debug.Log("Unrealized or Unknown Animation Type");
                    break;
            }
        }

		public static void SolidMelt(GameObject go, float rate) { 
			go.transform.localScale *= rate;
		}

		public static void LiquidChangeColor(GameObject go, Color color)
		{
			go.GetComponent<MeshRenderer>().material.color = color;
		}

		public static void LiquidReduce(GameObject go, float rate)
		{
			go.transform.localScale = new Vector3(go.transform.localScale.x, go.transform.localScale.y * rate, go.transform.localScale.z);
		}
    }
}
