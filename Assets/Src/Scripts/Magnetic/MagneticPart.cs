using System;
using System.Collections.Generic;
using System.Linq;
using DG.Tweening;
using UnityEngine;
using Random = UnityEngine.Random;
public class MagneticPart : MonoBehaviour {
    private const float DELAY_CHECK_NEAREST_PART = .5f;
    [Header("References")]
    public MeshRenderer partRenderer;
    public Rigidbody rb;
    public Transform hingePosition;
    public ParticleSystem attachParticle;
    public Collider myCollider;

    [Header("Data")]
    public int maxChildrenQuantity = 1;
    public float detectionDistance;
    public float attractionForce;
    public float attachDistance;

    protected readonly List<MagneticPart> _childrenPart = new();
    private float _attractForce;
    private float _checkNearestPartTimer;
    private int _deepValue;
    private ConfigurableJoint _joint;
    private MagneticPart _parentPart;
    private MagneticPart _target;
    public MagneticRoot _rootPart { get; protected set; }

    private void Awake() {
        this._checkNearestPartTimer = DELAY_CHECK_NEAREST_PART;
    }

    protected virtual void Update() {
        if (this.IsAttached()) {
            return;
        }

        this.NotAttachedBehavior();
    }

    private void FixedUpdate() {
        if (this._parentPart == true) {
            if (Vector3.Distance(this.transform.position, this._parentPart.transform.position) > 3) {
                this.transform.position = this._parentPart.transform.position;
            }
        }

        if (this._target == false || this.IsAttached()) {
            return;
        }

        Vector3 dir = this._target.hingePosition.position - this.transform.position;
        this.rb.AddForce(dir * this._attractForce);
    }

    private void OnDestroy() {
        this.transform.DOKill();
    }

        #if UNITY_EDITOR
    private void OnDrawGizmos() {
        if (this.IsAttached()) {
            return;
        }
        Gizmos.color = Color.yellow;
        Gizmos.DrawWireSphere(this.transform.position, this.detectionDistance);
    }
        #endif

    public static event Action<MagneticPart> OnPartAttached;
    public event Action<MagneticPart> OnPartAttachedNonStatic;
    public static event Action<MagneticPart> OnPartDetached;

    protected virtual bool IsAttached() {
        return this._parentPart == true;
    }

    private void NotAttachedBehavior() {
        this._checkNearestPartTimer -= Time.deltaTime;
        if (this._checkNearestPartTimer <= 0) {
            this._checkNearestPartTimer = DELAY_CHECK_NEAREST_PART;
            this._target = this.GetNearestAvailablePart();
        }

        if (this._target == false) {
            return;
        }

        float distance = Vector3.Distance(this.transform.position, this._target.hingePosition.position);
        this._attractForce = (1 - distance / this.detectionDistance) * this.attractionForce;
        if (distance > this.detectionDistance) { //target goes out of range
            this._target = null;
            this.GetNearestAvailablePart();
            return;
        }
        if (distance < this.attachDistance) {
            this.Attach(this._target);
        }
    }

    private MagneticPart GetNearestAvailablePart() {
        float nearestDistance = float.MaxValue;
        MagneticPart nearestPart = null;

        foreach (Collider col in Physics.OverlapSphere(this.transform.position, this.detectionDistance)) {
            if (col.gameObject == this.gameObject) {
                continue;
            }

            if (col.TryGetComponent(out MagneticPart tempPart) == false || tempPart.CanAttachTo() == false) {
                continue;
            }

            float tempDistance = Vector3.SqrMagnitude(tempPart.transform.position - this.transform.position);
            if (tempDistance >= nearestDistance) {
                continue;
            }

            nearestDistance = tempDistance;
            nearestPart = tempPart;
        }

        return nearestPart;
    }

    public void Attach(MagneticPart parentPart) {
        if (parentPart.CanAttachTo() == false) {
            return;
        }

        this._parentPart = parentPart;
        parentPart._childrenPart.Add(this);
        this._deepValue = parentPart._deepValue + 1;
        this._rootPart = parentPart._rootPart;
        this._rootPart.allChildren.Add(this);

        this._joint = this.gameObject.AddComponent<ConfigurableJoint>();
        this._joint.connectedBody = parentPart.rb;
        this._joint.autoConfigureConnectedAnchor = false;
        this._joint.connectedAnchor = parentPart.transform.InverseTransformVector(this._parentPart.hingePosition.position - parentPart.transform.position);
        this._joint.xMotion = ConfigurableJointMotion.Locked;
        this._joint.yMotion = ConfigurableJointMotion.Locked;
        this._joint.zMotion = ConfigurableJointMotion.Locked;

        this.rb.mass *= .1f;

        foreach (MagneticPart part in this._rootPart.allChildren) {
            if (part == parentPart || part._deepValue == this._deepValue) {
                continue;
            }

            Physics.IgnoreCollision(this.myCollider, part.myCollider);
        }

        //this.attachParticle.Play();

        OnPartAttached?.Invoke(this);
        this.OnPartAttachedNonStatic?.Invoke(this);
    }

    public virtual bool CanAttachTo() {
        return this.IsAttached() && this._childrenPart.Count < this.maxChildrenQuantity;
    }

    public virtual void Detach() {
        this._parentPart.DetachChild(this);
        this._rootPart.allChildren.Remove(this);

        Destroy(this._joint);
        this.DetachChildren();

        OnPartDetached?.Invoke(this);

        this._parentPart = null;
        this._rootPart = null;
    }

    private void DetachChild(MagneticPart child) {
        if (this._childrenPart.IsNullOrEmpty() == false && this._childrenPart.Contains(child)) {
            this._childrenPart.Remove(child);
        }
    }

    private void DetachChildren() {
        foreach (MagneticPart child in this._childrenPart) {
            child.Detach();
        }
    }

    protected int IsFurthestChild(out MagneticPart part, int deepValue = 0) {
        deepValue++;

        if (this._childrenPart.Count == 0) {
            part = this;
            return deepValue;
        }

        int bestDeepValue = 0;
        MagneticPart bestChild = null;
        foreach (MagneticPart child in this._childrenPart) {
            int tempDeepValue = child.IsFurthestChild(out part, deepValue);
            if (tempDeepValue > bestDeepValue) {
                bestDeepValue = tempDeepValue;
                bestChild = part;
            }
        }

        part = bestChild;
        return bestDeepValue;
    }

    public MagneticPart GetRandomChildren() {
        List<MagneticPart> partAvailable = this._childrenPart.ToList();
        partAvailable.Add(this);
        MagneticPart result = partAvailable.GetRandom();
        return result == this ? result : result.GetRandomChildren();
    }

    public MagneticPart GetRandomToAttach() {
        if (this._childrenPart.Count == 0) {
            return this;
        }
        if (this.CanAttachTo() && Random.value > .5f) {
            return this;
        }
        return this._childrenPart.GetRandom().GetRandomToAttach();
    }
}