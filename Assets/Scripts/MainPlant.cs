using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MainPlant : MonoBehaviour, IDamageable
{
    [SerializeField] private int _maxHealth;
    [SerializeField] private int _enemyLayer;

    private int _health;
    private void Start()
    {
        _health = _maxHealth;
    }

    public void TakeDamage(int amount)
    {
        _health -= amount;
        if (_health <= 0)
            DestroyFlower();
    }
    private void DestroyFlower()
    {
        //To-Do End Level
        Debug.Log("apuaaa ");
    }
}
