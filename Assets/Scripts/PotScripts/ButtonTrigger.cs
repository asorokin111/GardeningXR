using UnityEngine;

/// <summary>
/// Ingame button base, collision detection against top part of button.
/// </summary>
public class ButtonTrigger: MonoBehaviour
{
    // Master script in parent.
    IngameButton ingameButton;

    private void Awake()
    {
        ingameButton = this.GetComponentInParent<IngameButton>();
    }

    // This triggers main button script.
    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.CompareTag("IngameButton"))
        {
            ingameButton.OnButtonPressed();
        }
    }
}
