namespace UCE
{
    using UnityEngine;
    using VRTK;

    public class Dropper : VRTK_InteractableObject
    {
        private bool hasWater = true;

        public override void StartUsing(VRTK_InteractUse usingObject)
        {
            base.StartUsing(usingObject);
            Debug.Log("StartUsing");
            transform.Find("water").gameObject.SetActive(!hasWater);
            hasWater = !hasWater;
        }

        public override void StopUsing(VRTK_InteractUse usingObject)
        {
            base.StopUsing(usingObject);
            Debug.Log("StopUsing");
        }
    }
}
