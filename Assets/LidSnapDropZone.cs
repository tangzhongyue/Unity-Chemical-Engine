namespace UCE
{
    using UnityEngine;
    using VRTK;

    public class LidSnapDropZone : VRTK_SnapDropZone
    {   
        public override void OnObjectSnappedToDropZone(SnapDropZoneEventArgs e)
        {
            base.OnObjectSnappedToDropZone(e);
            Burner burner = transform.parent.gameObject.GetComponent<Burner>();
            burner.PutOutFire();
            burner.lidIsOn = true;
        }

        public override void OnObjectUnsnappedFromDropZone(SnapDropZoneEventArgs e)
        {
            base.OnObjectUnsnappedFromDropZone(e);
            Burner burner = transform.parent.gameObject.GetComponent<Burner>();
            burner.lidIsOn = false;
        }
    }
}