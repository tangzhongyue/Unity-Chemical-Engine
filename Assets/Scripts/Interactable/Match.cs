namespace UCE
{
    using System.Collections;
    using UnityEngine;
    using VRTK;

    // TODO: wait a few seconds and put off fire
    public class Match : VRTK_InteractableObject
    {
        [HideInInspector]
        public bool onFire = false;

        public override void StartUsing(VRTK_InteractUse usingObject)
        {
            base.StartUsing(usingObject);
            SetFire();
        }

        void SetFire()
        {
            onFire = true;
            transform.Find("fire").gameObject.SetActive(true);
        }
    }
}