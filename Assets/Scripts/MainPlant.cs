using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MainPlant : MonoBehaviour
{
    [SerializeField] private int _maxHealth;
    private int _health;
    private void Start()
    {
        _health = _maxHealth;
    }
    public void Damage(int amount)
    {
        _health -= amount;
        if (_health <= 0)
            DestroyFlower();
    }
    private void DestroyFlower()
    {
        Debug.Log("apuaaa ");
    }
}
