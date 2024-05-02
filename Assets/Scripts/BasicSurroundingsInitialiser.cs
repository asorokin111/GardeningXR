using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.XR.ARFoundation;
using UnityEngine.XR.Interaction.Toolkit;
using UnityEngine.XR.Interaction.Toolkit.Interactors;

public class BasicSurroundingsInitialiser : MonoBehaviour
{
    public XRRayInteractor interactor;
    public ARAnchorManager anchorManager;
    public GameObject prefab;
    public InputActionReference inputAction;

    private bool _hasBeenSpawned = false;

    private void OnEnable()
    {
        inputAction.action.performed += ReenableDraw;
        inputAction.action.canceled += ReenableDraw;
    }

    private void OnDisable()
    {
        inputAction.action.performed -= ReenableDraw;
        inputAction.action.canceled -= ReenableDraw;
    }

    private void ReenableDraw(InputAction.CallbackContext ctx)
    {
        if (!_hasBeenSpawned)
            SpawnAnchor(null);
    }

    private void SpawnAnchor(BaseInteractionEventArgs _)
    {
        interactor.TryGetCurrent3DRaycastHit(out RaycastHit hit);

        if (hit.collider == null) return;

        Instantiate(prefab, hit.collider.transform.position, Quaternion.identity);
        _hasBeenSpawned = true;
    }
}
