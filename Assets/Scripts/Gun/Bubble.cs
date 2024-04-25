using System;
using System.Collections;
using System.Collections.Generic;
using Nrjwolf.Tools.AttachAttributes;
using UnityEngine;

public class Bubble : MonoBehaviour
{
	[GetComponent] [SerializeField] private Rigidbody      rb;
	private                                 Action<Bubble> _onCollisionAction;

	private void OnCollisionEnter(Collision other)
	{
		_onCollisionAction(this);
	}

	public void Init(Action<Bubble> action) => _onCollisionAction = action;
	public void SetVelocity(Vector3 velocity) => rb.velocity = velocity;
}
