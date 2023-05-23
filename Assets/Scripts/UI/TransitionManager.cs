using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class TransitionManager : MonoBehaviour
{
    public void ChangeScene(string sceneName)
    {
        SceneManager.LoadScene(sceneName);
    }

    public void LoadGame()
    {
        AdsManager.instance.PlayAdsInterstitial();
        AdsManager.instance.OnShowAdsComplete += () => ChangeScene("MainGame");
    }
}
