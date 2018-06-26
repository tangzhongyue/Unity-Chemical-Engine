namespace UCE
{
    using UnityEngine;
    using VRTK;

    public class CoverGlassSnapDropZone : VRTK_SnapDropZone
    {
        public override void OnObjectSnappedToDropZone(SnapDropZoneEventArgs e)
        {
            base.OnObjectSnappedToDropZone(e);
            TipBoard.Progress(4, 2);
        }
    }
}