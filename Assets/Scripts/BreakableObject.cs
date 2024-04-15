using System.Collections;
using UnityEngine;

public class BreakableObject : MonoBehaviour
{
    [SerializeField] private GameObject _brokenPrefab;

    private Rigidbody _rigidbody;
    private Vector3   _currentVelocity;

    private GameObject _brokenInstance;

    private void Start()
    {
        _rigidbody = GetComponent<Rigidbody>();

        StartCoroutine(UpdateVelocityEverySecond());
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.CompareTag("Player"))
            return;

        if (collision.relativeVelocity.magnitude > 5f){
            BreakObject();
            TryToApplyCurrentForce();
            Destroy(gameObject);
        }
    }

    private void BreakObject()
    {
        _brokenInstance = Instantiate(_brokenPrefab, gameObject.transform.position, gameObject.transform.rotation);
    }

    private void TryToApplyCurrentForce()
    {
        try
        {
            ApplyCurrentForce();
        } catch (System.Exception e)
        {
            Debug.LogError(e.ToString());
        }
    }

    private void ApplyCurrentForce()
    {
        _brokenInstance.GetComponent<ForceForBrokenObject>().AddForceToAllParts(_currentVelocity);
    }

    private void UpdateCurrentVelocity() => _currentVelocity = _rigidbody.velocity;

    private IEnumerator UpdateVelocityEverySecond()
    {
        while(true)
        {
            yield return new WaitForSeconds(1f);
            UpdateCurrentVelocity();
        }
    }
}