using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FaceCamera : MonoBehaviour
{
    [HideInInspector] public Camera _cameraToLookAt;
    [SerializeField] private bool _freezeXZ;
    private void Update()
    {
        if(_cameraToLookAt == null)
            return;

        if (_freezeXZ)
        {
            transform.LookAt(new Vector3(0, _cameraToLookAt.transform.position.y, 0));
            return;
        }
        transform.LookAt(_cameraToLookAt.transform);
    }
}
