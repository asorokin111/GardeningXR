using UnityEngine;

/// <summary>
/// Drag and drop this component if you want to pass GameState
/// </summary>
public class GameStateComponent : MonoBehaviour
{
    [field: SerializeField] public GameState GameState { get; private set; }
}