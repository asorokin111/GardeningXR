using UnityEngine;
using UnityEngine.XR.Interaction.Toolkit.Interactables;

public class Sponge : MonoBehaviour
{
    public delegate void SpongeGrabbedAction();
    public static SpongeGrabbedAction OnSpongeGrabbed;
    private XRGrabInteractable _interactable;

    private void Awake()
    {
        _interactable = GetComponent<XRGrabInteractable>();
        _interactable.selectEntered.AddListener((_) => OnSpongeGrabbed?.Invoke());
    }
}
