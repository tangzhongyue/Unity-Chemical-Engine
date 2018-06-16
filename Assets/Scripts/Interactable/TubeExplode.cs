namespace UCE
{
    using System.Collections;
    using UnityEngine;

    public class TubeExplode : MonoBehaviour
    {
        public float explosionForce = 70f;
        public float explosionRadius = 1.6f;
        public int explosionNumber = 21;
        public bool doPresentation = false;
        public GameObject tube;

        IEnumerator ExplodePresentation()
        {
            yield return new WaitForSeconds(1f);
            Explode();
        }

        void Explode()
        {
            tube.SetActive(false);
            for (int i = 1; i < explosionNumber + 1; i++)
            {
                Transform trans = transform.Find("break_" + i.ToString());
                trans.gameObject.SetActive(true);
                Rigidbody rb = trans.GetComponent<Rigidbody>();
                rb.AddExplosionForce(explosionForce, transform.position, explosionRadius);
            }
        }

        // Use this for initialization
        void Start()
        {
            if (doPresentation)
                StartCoroutine(ExplodePresentation());
        }
    }
}