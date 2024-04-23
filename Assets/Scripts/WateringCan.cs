using UnityEngine.XR.Interaction.Toolkit;
using UnityEngine.XR.Interaction.Toolkit.Interactables;

public sealed class WateringCan : ParticleDropper
{
    public delegate void WateringCanPickupAction();
    public static event WateringCanPickupAction OnWateringCanFirstPickedUp;

    private XRGrabInteractable _interactable;

    protected override void Awake()
    {
        base.Awake();
        _interactable = GetComponent<XRGrabInteractable>();
    }

    private void OnEnable()
    {
        _interactable.firstSelectEntered.AddListener((SelectEnterEventArgs _) => OnWateringCanFirstPickedUp?.Invoke());
    }

    private void OnDisable()
    {
        _interactable.firstSelectEntered.RemoveAllListeners();
    }

    protected override bool CheckTilt()
    {
        return _particleSystem.transform.position.y < transform.position.y;
    }
}
