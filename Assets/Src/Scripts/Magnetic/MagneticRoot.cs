using System.Collections.Generic;
using DG.Tweening;
public class MagneticRoot : MagneticPart {
    public List<MagneticPart> allChildren { get; set; } = new();

    protected void Awake() {
        this._rootPart = this;
    }

    protected void OnDestroy() {
        this.transform.DOKill();
    }

    protected override bool IsAttached() {
        return true;
    }

    public bool IsEmpty() {
        return this._childrenPart.Count == 0;
    }

    public MagneticPart GetFurthestPart() {
        this.IsFurthestChild(out MagneticPart furthestChild);

        return furthestChild;
    }
}