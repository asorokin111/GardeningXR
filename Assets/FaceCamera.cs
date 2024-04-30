using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FaceCamera : MonoBehaviour
{
    [HideInInspector] public Camera cameraToLookAt;
    private void Update()
    {
        if(cameraToLookAt == null)
            return;
        transform.LookAt(cameraToLookAt.transform);
    }
}
