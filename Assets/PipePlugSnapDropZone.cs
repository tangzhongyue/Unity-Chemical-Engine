namespace UCE
{
    using UnityEngine;
    using VRTK;

    public class PipePlugSnapDropZone : VRTK_SnapDropZone
    {

        public override void OnObjectSnappedToDropZone(SnapDropZoneEventArgs e)
        {
            base.OnObjectSnappedToDropZone(e);
            Debug.Log(e.snappedObject.name + " in");
            AirTransmit air = transform.GetComponent<AirTransmit>();
            AirTransmit.Connect(air, e.snappedObject.GetComponent<AirTransmit>());
        }

        public override void OnObjectUnsnappedFromDropZone(SnapDropZoneEventArgs e)
        {
            base.OnObjectUnsnappedFromDropZone(e);
            Debug.Log(e.snappedObject.name + " out");
            AirTransmit air = transform.GetComponent<AirTransmit>();
            AirTransmit.Disconnect(air, e.snappedObject.GetComponent<AirTransmit>());
        }
    }
}