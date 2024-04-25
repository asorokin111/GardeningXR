using System;
using System.Collections;
using System.Collections.Generic;
using Nrjwolf.Tools.AttachAttributes;
using UnityEngine;
using Random = UnityEngine.Random;

public class Bubble : MonoBehaviour
{
	[GetComponent] [SerializeField] private Rigidbody rb;
	[SerializeField]                private float     radius;
	[SerializeField]                private float     perlinNoiseDelta;
	
	private Action<Bubble> _onCollisionAction;
	
	private Vector3 _shotDirection;
	private float   _speed;

	private void OnCollisionEnter(Collision other)
	{
		_onCollisionAction(this);
	}

	public void Init(Action<Bubble> action) => _onCollisionAction = action;
	public void SetVelocity(Vector3 velocity)
	{
		rb.velocity = velocity;

		_shotDirection = velocity.normalized;
		_speed         = velocity.magnitude;
		radius         *= Mathf.Sign(Random.Range(-10, 10));
	}

	private void FixedUpdate()
	{
		RecalculateVelocity(Time.time);
	}

	private void RecalculateVelocity(float t)
	{
		float radiusDelta = Mathf.PerlinNoise1D(t) * perlinNoiseDelta;
		var radiusVector =  new Vector3(
			Mathf.Cos(t) * radius + radiusDelta,
			Mathf.Sin(t) * radius + radiusDelta,
			_speed
		);
		
		rb.velocity = Quaternion.LookRotation(radiusVector, _shotDirection) * radiusVector;
	}
}
