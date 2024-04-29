using System.Collections;
using System.Collections.Generic;
using Nrjwolf.Tools.AttachAttributes;
using UnityEngine;

public class ShakingPlane : MonoBehaviour
{
	[GetComponent][SerializeField] private Rigidbody rb;
	[SerializeField] private float shakeForce;
	
	// will be called on button press
	public void AddShakeForce()
	{
		rb.AddForce(rb.transform.forward * shakeForce, ForceMode.Impulse);
	}
}
