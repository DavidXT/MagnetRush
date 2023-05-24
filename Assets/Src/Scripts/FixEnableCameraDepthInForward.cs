using UnityEngine;
[ExecuteInEditMode]
public class FixEnableCameraDepthInForward  : MonoBehaviour{
    private void Start(){
        Camera.main.depthTextureMode |= DepthTextureMode.Depth;
    }
}