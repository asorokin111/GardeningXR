using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.XR.ARFoundation;
using UnityEngine.XR.ARSubsystems;
using UnityEngine.XR.Interaction.Toolkit.Interactors;
using UnityEngine.XR.Interaction.Toolkit;

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

    public async void SpawnAnchor(BaseInteractionEventArgs _)
    {
        interactor.TryGetCurrent3DRaycastHit(out RaycastHit hit);

        var rotation = hit.normal != Vector3.zero ? Quaternion.LookRotation(hit.normal) : Quaternion.identity;
        Pose hitPos = new Pose(hit.point, rotation);

        if (!hit.collider.gameObject.TryGetComponent(out ARPlane plane)) return;
        if (plane.classifications != PlaneClassifications.DoorFrame) return;

        var result = await anchorManager.TryAddAnchorAsync(hitPos);
        bool success = result.TryGetResult(out ARAnchor anchor);

        if (!success) return;

        Instantiate(prefab, anchor.pose.position, Quaternion.identity);

        _hasBeenSpawned = true;
    }
}
