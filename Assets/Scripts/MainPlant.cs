using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MainPlant : MonoBehaviour, IDamageable
{
    [SerializeField] private int _maxHealth;
    [SerializeField] private float _damageRadius;
    [SerializeField] private int _damage;
    [SerializeField] private int _enemyLayer;
    private Collider _collider;

    private int _health;
    private void Start()
    {
        _health = _maxHealth;
        _collider = GetComponent<Collider>();
    }

    //private void Update()
    //{
    //    if (Input.GetKeyUp(KeyCode.Space))
    //    {
    //        Collider[] colliders = Physics.OverlapSphere(transform.position, _damageRadius);
    //        foreach (Collider hit in colliders)
    //        {
    //            if (hit.gameObject.CompareTag("WeedEnemy"))
    //            {
    //                if(hit.TryGetComponent(out IDamageable damageable) )
    //                {
    //                    damageable.TakeDamage(_damage);
    //                }
    //            }
    //        }
    //    }
    //}
    public void TakeDamage(int amount)
    {
        _health -= amount;
        if (_health <= 0)
            DestroyFlower();
    }
    private void DestroyFlower()
    {
        Debug.Log("apuaaa ");
    }
    private void OnDrawGizmosSelected()
    {
        Gizmos.DrawWireSphere(transform.position, _damageRadius);
    }
}
