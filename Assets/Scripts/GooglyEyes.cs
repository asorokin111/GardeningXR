using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GooglyEyes : MonoBehaviour
{
    public Transform Eye;
    [Range(0.5f, 10f)]
    public float Speed = 1f;
    [Range(0f, 10f)]
    public float GravityMultiplier = 1f;
    [Range(0.01f, 0.98f)]
    public float Bounciness = 0.4f;


    void Start()
    {
    }

    void Update()
    {
        //Calculate the allowed distance

    }
}
