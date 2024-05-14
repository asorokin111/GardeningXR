using UnityEngine;
using UnityEngine.XR.Interaction.Toolkit.Interactables;

public class DirtPacket : ParticleDropper
{
    public delegate void DirtPacketPickUpAction();
    public static DirtPacketPickUpAction OnDirtPacketFirstPickedUp;
    private bool _alreadyPickedUp = false;

    private XRGrabInteractable _interactable;
    protected override bool CheckTilt()
    {
        var angles = transform.rotation.eulerAngles;
        int dropAngle = 100;
        // Prevents the seed packet from acting weirdly with large rotation values
        int modulo = 360 - dropAngle;
        return Mathf.Abs(angles.x) % modulo >= dropAngle || Mathf.Abs(angles.z) % modulo >= dropAngle;
    }

    protected override void Awake()
    {
        base.Awake();
        _interactable = GetComponent<XRGrabInteractable>();
    }

    private void OnEnable()
    {
        _interactable.firstSelectEntered.AddListener((_) => {
            if (_alreadyPickedUp) return;
            OnDirtPacketFirstPickedUp?.Invoke();
            _alreadyPickedUp = true;
        });
    }

    private void OnDisable()
    {
        _interactable.firstSelectEntered.RemoveAllListeners();
    }
}
