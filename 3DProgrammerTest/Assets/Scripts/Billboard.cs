using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Billboard : MonoBehaviour
{
    private Camera cam;

    void Awake()
    {
        cam = Camera.main;
    }

    void Update()
    {
        transform.rotation = Quaternion.LookRotation(cam.transform.forward);
    }
}
