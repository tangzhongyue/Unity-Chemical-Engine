namespace UCE
{
    using UnityEngine;
    using VRTK;

    public class CuttonSnapDropZone : VRTK_SnapDropZone
    {
        public override void OnObjectSnappedToDropZone(SnapDropZoneEventArgs e)
        {
            TipBoard.Progress(1, 4);
        }
    }
}
