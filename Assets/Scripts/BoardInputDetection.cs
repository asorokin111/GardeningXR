using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class BoardInputDetection : MonoBehaviour
{
    [SerializeField]
    private NewControls _inputActions;
    private Transform _currentNote;
    private void Start()
    {
        _inputActions = new NewControls();
        _inputActions.Interactions.Enable();
    }
    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag("TaskNote"))
        {
            Debug.Log("Note");
        }
    }
    private void HandleNote(Transform transform)
    {
        _inputActions.Interactions.Grip.canceled += PlaceNote;
        _currentNote = transform;
    }
    private void PlaceNote(InputAction.CallbackContext context)
    {
        
    }
}
