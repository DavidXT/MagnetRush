using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class LevelButton : MonoBehaviour
{
    public string playerPrefName;
    public GameObject lockSprite;

    void Start()
    {
        if(PlayerPrefs.GetInt(playerPrefName) == 0)
        {
            lockSprite.SetActive(true);
            gameObject.GetComponent<Button>().interactable = false;
        }
    }
}
