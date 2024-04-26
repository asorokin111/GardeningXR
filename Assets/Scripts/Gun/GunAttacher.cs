using System.Collections;
using System.Collections.Generic;
using Nrjwolf.Tools.AttachAttributes;
using UnityEngine;
using UnityEngine.InputSystem;

public class GunAttacher : MonoBehaviour
{
    [SerializeField] private InputActionReference fireButton;
    [GetComponent] [SerializeField] private PunGun punGun;

    private FixedJoint _joint;
    
    // will be called onSelected;
    public void AttachToController(Rigidbody controllerRb)
    {
        transform.position = controllerRb.gameObject.transform.position;
        transform.forward  = controllerRb.gameObject.transform.forward;

        _joint = gameObject.AddComponent<FixedJoint>();
        _joint.connectedBody = controllerRb;
        
        fireButton.action.started += StartShooting;
        fireButton.action.canceled += StopShooting;

        Debug.Log("Attaching");
    }

    //on deselect
    public void RemoveAttachment()
    {
        Destroy(_joint);
        
        fireButton.action.started  -= StartShooting;
        fireButton.action.canceled -= StopShooting;
        
        Debug.Log("Removing attachment");
    }
    
    private void StartShooting(InputAction.CallbackContext _) => punGun.StartShooting();
    private void StopShooting(InputAction.CallbackContext  _) => punGun.StopShooting();
}
