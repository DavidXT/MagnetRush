using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraFollow : MonoBehaviour
{
    public Transform target;
    public Vector3 StartPos;
    public float offSet;
    private void Start()
    {
        StartPos = transform.position;
    }
    void Update()
    {
        transform.position = new Vector3(StartPos.x, StartPos.y, target.position.z + offSet);
    }

}
