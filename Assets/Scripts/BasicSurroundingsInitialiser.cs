using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.XR.ARFoundation;
using UnityEngine.XR.ARSubsystems;
using UnityEngine.XR.Interaction.Toolkit.Interactors;
using UnityEngine.XR.Interaction.Toolkit;

public class BasicSurroundingsInitialiser : MonoBehaviour
{
    public LayerMask grassLayer;
    public XRRayInteractor interactor;
    public ARAnchorManager anchorManager;
    public GameObject prefab;
    public InputActionReference uhu;

    public Mesh mesh;
    public Material material;

    private bool _hasBeenSpawned = false;

    private void OnEnable()
    {
        uhu.action.performed += ReenableDraw;
        uhu.action.canceled += ReenableDraw;
    }

    private void OnDisable()
    {
        uhu.action.performed -= ReenableDraw;
        uhu.action.canceled -= ReenableDraw;
    }

    private void ReenableDraw(InputAction.CallbackContext ctx)
    {
        if (!_hasBeenSpawned)
            SpawnAnchor(null);
    }

    public async void SpawnAnchor(BaseInteractionEventArgs args)
    {
        interactor.TryGetCurrent3DRaycastHit(out RaycastHit hit);

        Pose hitPos = new Pose(hit.point, Quaternion.LookRotation(hit.normal));

        if (!hit.collider.gameObject.TryGetComponent(out ARPlane plane)) return;
        if (plane.classifications != PlaneClassifications.DoorFrame) return;

        var result = await anchorManager.TryAddAnchorAsync(hitPos);
        bool success = result.TryGetResult(out ARAnchor anchor);

        if (!success) return;

        Instantiate(prefab, anchor.pose.position, anchor.pose.rotation);
    }
}
