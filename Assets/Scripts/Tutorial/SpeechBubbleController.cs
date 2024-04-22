using UnityEngine;

public class SpeechBubbleController : MonoBehaviour
{
    [SerializeField] private Transform _targetToFace;
    [SerializeField] private Transform _defaultPosition;
    [SerializeField] private float _maxIgnoreDistance;
    [SerializeField] private float _transitionSmoothTime;

    private Vector3 _currentVelocity; // For SmoothDamp

    private void Start()
    {
        transform.position = _defaultPosition.position;
    }

    private void Update()
    {
        // Move to default position if they are too far
        if (Vector3.Distance(transform.position, _defaultPosition.position) >= _maxIgnoreDistance)
        {
            transform.position = Vector3.SmoothDamp(transform.position, _defaultPosition.position, ref _currentVelocity, _transitionSmoothTime);
        }
    }
    private void LateUpdate()
    {
       transform.LookAt(_targetToFace.position);  // Rotate towards target 
    }
}
