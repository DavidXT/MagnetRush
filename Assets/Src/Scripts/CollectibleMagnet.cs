using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CollectibleMagnet : MonoBehaviour {
    [Header("References")]
    public Collider myCollider;

    public void DisableCollider() {
        this.myCollider.enabled = false;
    }
}
