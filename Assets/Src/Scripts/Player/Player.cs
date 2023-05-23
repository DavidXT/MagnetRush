using System;
using UnityEngine;
public class Player : MonoBehaviour {
    [Header("References")]
    public PlayerMovement playerMovement;
    public MagneticRoot magneticRoot;

    [Header("Data")]
    public int startingMagnetPart = 1;

    private void Start() {
        this.InstantiateStartingPart();
    }

    private void InstantiateStartingPart() {
        MagneticPart parent = this.magneticRoot;
        for (int i = 0; i < this.startingMagnetPart; i++) {
            MagneticPart newPart = Instantiate(ResourcesManager.Instance.magneticPartPrefab, this.transform.position, this.transform.rotation, null);
            Debug.Log(parent);
            newPart.Attach(parent);
            parent = newPart;
        }
    }

    private void OnTriggerEnter(Collider other) {
        if (other.TryGetComponent(out MagneticPart magneticPart)) {
            this.magneticRoot.AttachToRandom(magneticPart);
        }
    }
}