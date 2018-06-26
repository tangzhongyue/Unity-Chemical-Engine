namespace UCE
{
    using UnityEngine;
    using VRTK;

    public class BottleSnapDropZone : VRTK_SnapDropZone
    {
        public override void OnObjectSnappedToDropZone(SnapDropZoneEventArgs e)
        {
            base.OnObjectSnappedToDropZone(e);
            TipBoard.Progress(4, 3);
        }
    }
}