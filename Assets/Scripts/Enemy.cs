using System;
using System.Collections;
using UnityEngine;

public class Enemy : MonoBehaviour, IDamageable
{
    private Action<Enemy> _onDieAction;

    [Header("Health & Damage")]
    [SerializeField] private ProgressBar _healthBar; //The UI object that will be places at a real Canvas to gain perfomance
    [SerializeField] private int _maxHealth;
    private float _health;
    [SerializeField] private float _damageRadius;
    [SerializeField] private int _damage;

    [SerializeField, Range(0f, 10f)] private int _damageFrequency;
    private WaitForSeconds _waitToGiveDamage;

    [Header("Jumping")]
    [SerializeField] private float _jumpForce;
    [SerializeField, Range(0f, 15f)] private int _secondUntilNextJump;
    private static WaitForSeconds _waitForNextJump;

    [Header("Deactivation Parameters")]
    [SerializeField, Range(0f, 30f)] private int _secondUntilDeactivation;

    private Rigidbody _rb;

    private Transform _target;

    private Canvas _UICanvas;
    private Camera _mainCamera;

    private bool _isGrounded;
    private bool _isDetectingCollisions;
    private bool _isAttacking;

    public void Create(Transform target)
    {
        _rb = GetComponent<Rigidbody>();
        _target = target;

        _waitForNextJump = new WaitForSeconds(_secondUntilNextJump);
        _waitToGiveDamage = new WaitForSeconds(_damageFrequency);

        _mainCamera = Camera.main;
        _UICanvas = GameObject.Find("World Optimization-Canvas").GetComponent<Canvas>();
    }
    public void Initialize(Action<Enemy> action)
    {
        _onDieAction = action;
        _health = _maxHealth;
        SetUpHealthBar();

        _isGrounded = true;
        MakeAJump();
    }
    private void SetUpHealthBar()
    {
        _healthBar.transform.SetParent(_UICanvas.transform);
        _healthBar.GetComponent<FaceCamera>().cameraToLookAt = _mainCamera;
        _healthBar.SetProgress(_health / _maxHealth);
    }

    public void TakeDamage(int amount)
    {
        _health -= amount;
        _healthBar.SetProgress(_health / _maxHealth);
        if (_health <= 0)
        {
            _onDieAction?.Invoke(this);
            _onDieAction = null;
        }
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

    #region Jumping
    private void MakeAJump()
    {
        transform.LookAt(_target);
        AddForceAtAngle(_jumpForce);
        _isDetectingCollisions = true;
    }

    IEnumerator WaitUntilNextJump()
    {
        yield return _waitForNextJump;
        MakeAJump();
    }

    private void AddForceAtAngle(float force)
    {
        //float zcomponent = Mathf.Tan(angle * Mathf.PI / 180) * force * 0.2f;
        //float ycomponent = Mathf.Sin(angle * Mathf.PI / 180) * force * 0.8f;
        //float xcomponent = Mathf.Cos(angle * Mathf.PI / 180) * force;
        Vector3 averageVector = (transform.forward + transform.up) / 2;

        _rb.AddForce(averageVector * force);
        _isGrounded = false;
    }
    #endregion

    private bool TryFindMainPlantNearby(out MainPlant plant)
    {
        Collider[] overlaps = Physics.OverlapSphere(transform.position, _damageRadius);
        if (overlaps.Length != 0)
        {
            foreach (Collider col in overlaps)
            {
                if (col.TryGetComponent<MainPlant>(out MainPlant mainPlant))
                {
                    plant = mainPlant;
                    return true;
                }
            }
        }
        plant = null;
        return false;
    }

    IEnumerator DamageMainPlant()
    {
        _isAttacking = true;
        while (true)
        {
            if (TryFindMainPlantNearby(out MainPlant mainPlant))
            {
                Debug.Log("Found Plant");
                yield return _waitToGiveDamage;
                mainPlant.TakeDamage(_damage);
            }
            else
            {
                _isAttacking = false;
                break;
            }
        }
        StartCoroutine(WaitUntilNextJump());
    }
    IEnumerator StartDeactivationCount()
    {
        int i = 0;
        while (i < _secondUntilDeactivation)
        {
            i++;
            yield return new WaitForSeconds(1f);
            if (_isGrounded)
                yield break;
        }
        Destroy(gameObject);
    }

    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.yellow;
        Gizmos.DrawWireSphere(transform.position, _damageRadius);
    }
}