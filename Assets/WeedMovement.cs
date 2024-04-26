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
    [SerializeField] private float _jumpAngle;

    [SerializeField, Range(0f,30f)] private int secondUntilDeactivation;
    private Rigidbody _rb;

    private bool _isGrounded;

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
    private void Start()
    {
        _rb = GetComponent<Rigidbody>();
        _isGrounded = true;
        MakeAJump();
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.CompareTag("WeedsGround"))
        {
            _isGrounded = true;
            _rb.velocity = Vector3.zero;
            _rb.angularVelocity = Vector3.zero;
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
        AddForceAtAngle(_jumpForce, _jumpAngle);
    }

    public void AddForceAtAngle(float force, float angle)
    {
        float xcomponent = Mathf.Cos(angle * Mathf.PI / 180) * force;
        float ycomponent = Mathf.Sin(angle * Mathf.PI / 180) * force;

        _rb.AddForce(ycomponent, 0, xcomponent);
        _isGrounded = false;
    }

    IEnumerator StartDeactivationCount()
    {
        int i = 0;
        while(i < secondUntilDeactivation)
        {
            i++;
            yield return new WaitForSeconds(1f);
            if (_isGrounded)
                yield break;
        }
        Destroy(gameObject);
    }
}