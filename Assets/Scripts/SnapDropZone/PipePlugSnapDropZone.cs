namespace UCE
{
    using UnityEngine;
    using VRTK;

    public class PipePlugSnapDropZone : VRTK_SnapDropZone
    {

        public override void OnObjectSnappedToDropZone(SnapDropZoneEventArgs e)
        {
            base.OnObjectSnappedToDropZone(e);
            AirTransmit air = transform.parent.GetComponent<AirTransmit>();
            AirTransmit.Connect(air, e.snappedObject.GetComponent<AirTransmit>());
            TipBoard.Progress(0, 0);
            TipBoard.Progress(1, 5);
        }

        public override void OnObjectUnsnappedFromDropZone(SnapDropZoneEventArgs e)
        {
            base.OnObjectUnsnappedFromDropZone(e);
            AirTransmit air = transform.parent.GetComponent<AirTransmit>();
            AirTransmit.Disconnect(air, e.snappedObject.GetComponent<AirTransmit>());
            TipBoard.Progress(1, 0);
            TipBoard.Progress(5, 0);
        }
    }
}