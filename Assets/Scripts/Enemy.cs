using System;
using System.Collections;
using UnityEngine;

public class Enemy : MonoBehaviour, IDamageable
{
    //Actions
    private Action _onJumpAction;
    private Action<Enemy> _onDieAction;

    //Components
    private Transform _target;
    private Canvas _UICanvas;
    private Camera _mainCamera;

    //Booleans 
    private bool _isAttacking = false;
    private bool _isJumping;
    private bool _isInitialized;

    //Coroutines
    private Coroutine _currentCoroutine;

    [Header("Health & Damage")]
    [SerializeField] private ProgressBar _healthBar; //The UI object that will be places at a real Canvas to gain perfomance
    [SerializeField] private int _maxHealth;
                     private float _health;
    [SerializeField] private float _damageRadius;
    [SerializeField] private int _damage;
    [SerializeField, Range(0f, 10f)] private int _damageFrequency;
                     private WaitForSeconds _waitToGiveDamage;

    [Header("Jumping")]
    [SerializeField] private EnemyGroundCheck _groundCheck;
    [SerializeField, Range(0f, 15f)] private int _secondUntilNextJump;
                     private static WaitForSeconds _waitForNextJump;
    [SerializeField] private float _jumpHeight;
    [SerializeField] private float _jumpLenght;
    [SerializeField] private float _gravityScale;

                     private float _Yvelocity;
                     private float _Zvelocity;

    private void Update()
    {
        if (!_isInitialized)
            return;

        if (_Yvelocity < -500f)
        {
            _onDieAction?.Invoke(this);
            _isInitialized = false;
        }

        if (_isAttacking)
            return;

        _Yvelocity += Physics.gravity.y * _jumpLenght * Time.deltaTime;
        _Zvelocity += _jumpLenght * Time.deltaTime;


        if (_groundCheck.isGrounded && _Yvelocity < 0)
        {
            _Yvelocity = 0;
            _Zvelocity = 0;
            if(TryFindMainPlantNearby(out var mainPlantNearby))
            {
                StartCoroutine(DamageMainPlant());
                return;
            }

            if(!_isJumping)
            {
                _isJumping = true;
                StartCoroutine(Jump());
            }
        }

        //Looking on the target
        Vector3 toTarget = _target.position - transform.localPosition;
        toTarget.y = 0;
        transform.localRotation = Quaternion.LookRotation(toTarget);

        transform.Translate(new Vector3(0, _Yvelocity, _Zvelocity) * Time.deltaTime);
    }

    #region Creation and Initialization
    /// <summary>
    /// Instead of Start() method
    /// </summary>
    /// <param name="target"></param>
    public void Create(Transform target)
    {
        _onJumpAction += () => StartCoroutine(Jump());

        _mainCamera = Camera.main;
        _UICanvas = GameObject.Find("World Optimization-Canvas").GetComponent<Canvas>();
        _target = target;

        _waitForNextJump = new WaitForSeconds(_secondUntilNextJump);
        _waitToGiveDamage = new WaitForSeconds(_damageFrequency);
    }

    public void Initialize(Action<Enemy> action)
    {
        _isInitialized = true;
        _onDieAction = action;
        _health = _maxHealth;
        SetUpHealthBar();
        StartCoroutine(Jump());
    }

    public void UnInitialize()
    {
        _isInitialized = false;
    }

    private void SetUpHealthBar()
    {
        _healthBar.transform.SetParent(_UICanvas.transform);
        _healthBar.GetComponent<FaceCamera>().cameraToLookAt = _mainCamera;
        _healthBar.SetProgress(_health / _maxHealth);
    }
    #endregion  

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

    #region Jumping
    IEnumerator Jump()
    {
        yield return _waitForNextJump;
        _Yvelocity = _jumpHeight;
        _isJumping = false;
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
                Debug.Log("Attacking plant");
                yield return _waitToGiveDamage;
                mainPlant.TakeDamage(_damage);
            }
            else
            {
                _isAttacking = false;
                break;
            }
        }
        StartCoroutine(Jump());
    }

    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.yellow;
        Gizmos.DrawWireSphere(transform.position, _damageRadius);
    }
}