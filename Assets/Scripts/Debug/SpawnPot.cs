using UnityEngine;
using UnityEngine.InputSystem;

public class SpawnPot : MonoBehaviour
{
    [SerializeField] private GameObject _potPrefab;
    [SerializeField] private float _spawnSpeed = 2;
    [SerializeField] private InputActionProperty _inputAction;

    private void Update()
    {
        if (_inputAction.action.WasPressedThisFrame())
        {
            GameObject spawnedPot = Instantiate(_potPrefab, transform.position, transform.rotation);
            Rigidbody potVelocity = spawnedPot.GetComponent<Rigidbody>();
            potVelocity.velocity = transform.forward * _spawnSpeed;
        }
    }
}
