using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Magnet : MonoBehaviour
{
   private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            if(GameManager.instance != null)
            {
                GameManager.instance.IncreaseMagnet(1);
            }
            Destroy(this.gameObject);
        }
    }
}
