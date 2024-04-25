using System.Collections;
using System.Collections.Generic;
using Nrjwolf.Tools.AttachAttributes;
using UnityEngine;
using UnityEngine.InputSystem;

public class GunAttacher : MonoBehaviour
{
    [SerializeField] private InputActionReference fireButton;
    [GetComponent] [SerializeField] private PunGun punGun;

    // will be called onSelected;
    public void AttachToController(Rigidbody controllerRb)
    {
        transform.position = controllerRb.transform.position;
        transform.forward  = controllerRb.transform.forward;

        GetComponent<Rigidbody>().isKinematic = true;
        GetComponent<Collider>().enabled      = false;
        var joint = gameObject.AddComponent<FixedJoint>();
        joint.connectedBody = controllerRb;
        
        fireButton.action.started += StartShooting;
        fireButton.action.canceled += StopShooting;
    }

    //on deselect
    public void RemoveAttachment()
    {
        GetComponent<FixedJoint>().connectedBody = null;
        GetComponent<Rigidbody>().isKinematic =  false;
        GetComponent<Collider>().enabled      =  true;
        
        fireButton.action.started  -= StartShooting;
        fireButton.action.canceled -= StopShooting;
    }
    
    private void StartShooting(InputAction.CallbackContext _) => punGun.StartShooting();
    private void StopShooting(InputAction.CallbackContext  _) => punGun.StopShooting();
}
