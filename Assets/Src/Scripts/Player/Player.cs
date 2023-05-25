using UnityEngine;
public class Player : MonoBehaviour {
    [Header("References")]
    public PlayerMovement playerMovement;
    public MagneticRoot magneticRoot;
    public ParticleSystem pSystem;
    public AudioSource collectiblesAudio;
    public ParticleSystem pSystemDeath;

    [Header("Data")]
    public int startingMagnetPart = 1;

    private void Start() {
        this.InstantiateStartingPart();
    }

    private void OnTriggerEnter(Collider other) {
        if (other.TryGetComponent(out CollectibleMagnet magnet)) {
            magnet.DisableCollider();
            collectiblesAudio.Play();
            pSystem.Play();

            MagneticPart parent = this.magneticRoot.GetRandomToAttach();
            MagneticPart newPart = Instantiate(ResourcesManager.Instance.magneticPartPrefab, parent.transform.position, parent.transform.rotation, null);
            newPart.gameObject.SetActive(false);
            newPart.Attach(parent);
                
            float height = Random.Range(4, 6f);
            Vector3 halfPointOffset = new(Random.Range(-2, 2f), Random.Range(-1, 1f), 0);
            float duration = .5f;
            this.StartCoroutine(BezierCurves.JumpIn(magnet.transform, parent.transform, Vector3.zero, height, halfPointOffset, duration, false, () => {
                newPart.gameObject.SetActive(true);
                Destroy(magnet.gameObject);
            }));
        }
    }

    private void InstantiateStartingPart() {
        MagneticPart parent = this.magneticRoot;
        for (int i = 0; i < this.startingMagnetPart; i++) {
            MagneticPart newPart = Instantiate(ResourcesManager.Instance.magneticPartPrefab, this.transform.position, this.transform.rotation, null);
            newPart.Attach(parent);
            parent = newPart;
        }
    }

}