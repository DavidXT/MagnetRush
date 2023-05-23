using UnityEngine;
public class ResourcesManager : MonoBehaviour {
    public static ResourcesManager Instance;

    [Header("Prefab")]
    public MagneticPart magneticPartPrefab;
    
    private void Awake() {
        if (Instance) {
            Destroy(this);
        }
        else {
            Instance = this;
        }
    }
}