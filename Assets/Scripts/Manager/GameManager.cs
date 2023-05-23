using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum States {Playing}
public class GameManager : MonoBehaviour
{
    public static GameManager instance;

    public States state;
    // Start is called before the first frame update
    void Start()
    {
        if (instance != null && instance != this)
        {
            Destroy(this.gameObject);
            return;
        }
        else
        {
            instance = this;
        }
    }
}
