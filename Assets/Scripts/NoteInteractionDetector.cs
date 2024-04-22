using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.XR.ARFoundation;
using UnityEngine.XR.Interaction.Toolkit;
using UnityEngine.XR.Interaction.Toolkit.Interactables;
using UnityEngine.XR.Interaction.Toolkit.Interactors;

[RequireComponent(typeof(Collider))]
public class NoteInteractionDetector: MonoBehaviour
{
    private XRGrabInteractable _grabInteractable;

    [SerializeField]
    private NewControls _inputActions;

    private Collider _boardCollider;

    private float _sphereCheckRadius;
    private bool _isHanging = false;

    private void Start()
    {
        _inputActions = new NewControls();
        _inputActions.Interactions.Enable();

        _grabInteractable = GetComponent<XRGrabInteractable>();
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag("CorkBoard"))
        {
            if (other.TryGetComponent<CorkBoard>(out var corkBoard))
            {
                _boardCollider = corkBoard.Collider;
                _grabInteractable.selectExited.AddListener(Snap);
            }
        }
    }
    private void OnTriggerExit(Collider other)
    {
        if (other.gameObject.CompareTag("CorkBoard"))
        {
            _boardCollider = null;
            _isHanging = false;
            _grabInteractable.selectExited.RemoveListener(Snap);
        }
    }

    private void SetSnapValues()
    {
        if (_boardCollider == null)
            return;
        transform.position = _boardCollider.ClosestPointOnBounds(transform.position);
        transform.rotation = _boardCollider.transform.rotation;
    }

    private void Snap(UnityEngine.XR.Interaction.Toolkit.SelectExitEventArgs args)
    {
        Debug.Log("Snap");
        _isHanging = true;
        SetSnapValues();
        //_grabInteractable.selectExited.AddListener(SnapOrForget);
    }
    //private void SnapOrForget(UnityEngine.XR.Interaction.Toolkit.SelectExitEventArgs args)
    //{
    //    Debug.Log("Snap or Forget");
    //    Collider[] hitColliders = Physics.OverlapSphere(transform.position, _sphereCheckRadius);
    //    if (hitColliders.Length > 0 && hitColliders.Contains(_boardCollider) && _isHanging)
    //    {
    //        SetSnapValues();
    //        Debug.Log("HA Ha");
    //    }
    //}
}
