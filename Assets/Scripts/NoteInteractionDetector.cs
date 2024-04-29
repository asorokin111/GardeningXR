using UnityEngine;
using UnityEngine.XR.Interaction.Toolkit;
using UnityEngine.XR.Interaction.Toolkit.Interactables;

[RequireComponent(typeof(Collider))]
public class NoteInteractionDetector: MonoBehaviour
{
    private XRGrabInteractable _grabInteractable;

    [SerializeField]
    private NewControls _inputActions;

    private Collider _boardCollider;

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

    private void Snap(SelectExitEventArgs args)
    {
        SetSnapValues();
    }
}
