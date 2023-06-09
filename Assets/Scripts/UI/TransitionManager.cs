using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

[DefaultExecutionOrder(100)]
public class TransitionManager : MonoBehaviour
{
    public void ChangeScene(string sceneName)
    {
        SceneManager.LoadScene(sceneName);
    }

    public void LoadLevel(string index)
    {
        if (AdsManager.instance != null)
        {
            if (AdsManager.instance.isInit)
            {
                AdsManager.instance.PlayAdsInterstitial();
                AdsManager.instance.OnShowAdsComplete = () => ChangeScene("Level" + index);
            }
            else
            {
                ChangeScene("Level" + index);
            }
        }
    }

    public void ReloadGame()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }

    public void LeaveGame()
    {
        Application.Quit();
    }
}
