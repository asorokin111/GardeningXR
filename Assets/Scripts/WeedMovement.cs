using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.AI;
using static UnityEngine.GraphicsBuffer;

public class WeedMovement : MonoBehaviour
{
    [SerializeField] private Transform _playerTransform;
    [SerializeField] private float _jumpForce;
    [SerializeField] private float _damageRadius;
    [SerializeField] private int _damage;
    

    [SerializeField, Range(0f, 15f)] private int _secondUntilNextJump;
    [SerializeField, Range(0f,30f)] private int _secondUntilDeactivation;
    [SerializeField, Range(0f, 10f)] private int _damageFrequency;

    private static WaitForSeconds _waitForNextJump;
    private WaitForSeconds _waitToGiveDamage;
    private Rigidbody _rb;

    private bool _isGrounded;
    private bool _isDetectingCollisions;
    private bool _isAttacking;

    private void Start()
    {
        _rb = GetComponent<Rigidbody>();
        _waitForNextJump = new WaitForSeconds(_secondUntilNextJump);
        _waitToGiveDamage = new WaitForSeconds(_damageFrequency);
        _isGrounded = true;
        transform.LookAt(_playerTransform.transform);
        MakeAJump();
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (!_isDetectingCollisions)
            return;

        if (collision.gameObject.CompareTag("WeedsGround"))
        {
            _isGrounded = true;
            _isDetectingCollisions = false;
            _rb.angularVelocity = Vector3.zero;
            _rb.velocity = Vector3.zero;

            if (TryFindMainPlantNearby(out MainPlant mainPlant))
            {
                StartCoroutine(DamageMainPlant());
                return;
            }

            StartCoroutine(WaitUntilNextJump());
        }
        else
        {
            _isGrounded = false;
            StartDeactivationCount();
        }
    }

    private void MakeAJump()
    {
        transform.LookAt(_playerTransform);
        AddForceAtAngle(_jumpForce);
        _isDetectingCollisions = true;
    }

    public void AddForceAtAngle(float force)
    {
        //float zcomponent = Mathf.Tan(angle * Mathf.PI / 180) * force * 0.2f;
        //float ycomponent = Mathf.Sin(angle * Mathf.PI / 180) * force * 0.8f;
        //float xcomponent = Mathf.Cos(angle * Mathf.PI / 180) * force;
        Vector3 averageVector = (transform.forward + transform.up)/2;

        _rb.AddForce(averageVector * force);
        _isGrounded = false;
    }

    private bool TryFindMainPlantNearby(out MainPlant plant)
    {
        Collider[] overlaps = Physics.OverlapSphere(transform.position, _damageRadius);
        if(overlaps.Length != 0 )
        {
            foreach(Collider col in overlaps)
            {
                if(col.TryGetComponent<MainPlant>(out MainPlant mainPlant))
                {
                    plant = mainPlant;
                    return true;
                }
            }
        }
        plant = null;
        return false;
    }
    
    IEnumerator StartDeactivationCount()
    {
        int i = 0;
        while(i < _secondUntilDeactivation)
        {
            i++;
            yield return new WaitForSeconds(1f);
            if (_isGrounded)
                yield break;
        }
        Destroy(gameObject);
    }
    IEnumerator WaitUntilNextJump()
    {
        yield return _waitForNextJump;
        MakeAJump();
    }
    IEnumerator DamageMainPlant()
    {
        _isAttacking = true;
        while (true)
        {
            if (TryFindMainPlantNearby(out MainPlant mainPlant))
            {
                Debug.Log("Found");
                yield return _waitToGiveDamage;
                mainPlant.Damage(_damage);
            }
            else
                break;
        }
        StartCoroutine(WaitUntilNextJump());
    }

    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.yellow;
        Gizmos.DrawWireSphere(transform.position, _damageRadius);
    }
}
//[SerializeField] private float _movementRange;
//[SerializeField] private float _maxJumpHeight;
//[SerializeField] float initialAngle;
//private Collider _currentGoalCollider;
//Queue<Vector3> tracePositions= new Queue<Vector3>();

//private void CreateTrajectory()
//{
//    Vector3 distance = _playerTransform.position - transform.position;
//    Vector3 tracePiece = distance.normalized;
//    int tracePiecesCount = (int)(distance.magnitude / tracePiece.magnitude);

//    for (int i = 0; i < tracePiecesCount; i++)
//    {
//        tracePositions.Enqueue(tracePiece * i);
//    }
//    tracePiecesCount = 0;

//}
//private void CalculateNextPos(out Vector3 distance)
//{
//    while (true)
//    {
//        distance = transform.position - _playerTransform.position;

//        Ray ray = new Ray(distance, Vector3.back);
//        if (Physics.Raycast(ray, out RaycastHit hit, _maxJumpHeight, _walkableLayer))
//        {
//            _currentGoalCollider = hit.collider;
//            break;
//        }
//    }  
//}