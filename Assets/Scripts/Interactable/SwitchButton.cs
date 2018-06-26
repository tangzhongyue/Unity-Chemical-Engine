namespace UCE
{
    using UnityEngine;
    using VRTK;

    public class SwitchButton : VRTK_InteractableObject
    {
        int equipment_id;

        public override void StartUsing(VRTK_InteractUse usingObject)
        {
            base.StartUsing(usingObject);
            equipment_id = 1 - equipment_id;
            if (equipment_id == 0)
            {
                transform.parent.Find("equipments-1").gameObject.SetActive(true);
                transform.parent.Find("equipments-2").gameObject.SetActive(false);
            }
            else
            {
                transform.parent.Find("equipments-2").gameObject.SetActive(true);
                transform.parent.Find("equipments-1").gameObject.SetActive(false);
            }
        }

        void Start()
        {
            if (transform.parent.Find("equipments-1").gameObject.activeInHierarchy)
                equipment_id = 0;
            else
                equipment_id = 1;
        }
    }
}