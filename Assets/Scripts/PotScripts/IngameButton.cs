using UnityEngine;

/// <summary>
/// Ingame button that works with slapping/pushing.
/// Relies to <seealso cref="IIngameButtonLogic"/>.
/// </summary>
public class IngameButton : MonoBehaviour
{
    [SerializeField]
    [Tooltip("Logic that this button will trigger")]
    GameObject targetLogic;

    /// <summary>
    /// <seealso cref="IIngameButtonLogic"/>
    /// </summary>
    public void OnButtonPressed() => targetLogic.GetComponent<IIngameButtonLogic>().TriggerAction(); 
}
