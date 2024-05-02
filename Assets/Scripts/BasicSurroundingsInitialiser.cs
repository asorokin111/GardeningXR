using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.XR.ARFoundation;
using UnityEngine.XR.ARSubsystems;
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
            SimpleInit(null);
    }

    private async void SpawnAnchor()
    {
        interactor.TryGetCurrent3DRaycastHit(out RaycastHit hit);

        var hitPose = new Pose(hit.point, Quaternion.LookRotation(hit.normal));

        if (!hit.collider.gameObject.TryGetComponent(out ARPlane plane)) return;
        if (plane.classifications != PlaneClassifications.DoorFrame) return;

        var result = await anchorManager.TryAddAnchorAsync(hitPose);
        bool success = result.TryGetResult(out ARAnchor anchor);

        if (!success) return;

        Instantiate(prefab, anchor.pose.position, Quaternion.identity);

        _hasBeenSpawned = true;
    }

    private void SimpleInit(BaseInteractionEventArgs _)
    {
        interactor.TryGetCurrent3DRaycastHit(out RaycastHit hit);

        if (hit.collider == null) return;

        Instantiate(prefab, hit.collider.transform.position, Quaternion.identity);
        _hasBeenSpawned = true;
    }
}
