using System;
using System.Collections;
using System.Collections.Generic;
using Nrjwolf.Tools.AttachAttributes;
using TMPro;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.InputSystem;
using UnityEngine.XR.Interaction.Toolkit.Interactables;

public class WaterGun : MonoBehaviour
{
	[SerializeField] private TMP_Text ammoText;

	[Header("Gun properties")] [SerializeField]
	private float fireDelay;

	[SerializeField]                private int        magazineCapacity;
	[SerializeField]                private int        totalAmmoAmount;
	[GetComponent] [SerializeField] private Animator   animator;
	[GetComponent] [SerializeField] private FixedJoint fixedJoint;
	[GetComponent] [SerializeField] private XRGrabInteractable grabInteractable;

	[Header("Bubble properties")] 
	[SerializeField] private Transform bubbleSpawnPoint;
	[SerializeField] private Rigidbody bubblePrefab;
	[SerializeField] private float     bubbleSpeed;

	private IEnumerator _fireCoroutine;
	private int         _currentAmmoInMagazine;
	private float       _lastShotTime;

	private bool _canShoot = true;

	private void Start()
	{
		_currentAmmoInMagazine = magazineCapacity;
		_fireCoroutine         = FireCoroutine();

		UpdateAmmoText();                                                            
	}

	// called by player
	public void StartShooting()
	{
		if (!_canShoot) return;
		if (_lastShotTime + fireDelay > Time.time) return;
		
		_fireCoroutine = FireCoroutine();
		StartCoroutine(_fireCoroutine);
	}
	
	// called by player
	public void StopShooting()  => StopCoroutine(_fireCoroutine);
	
	// called on select
	public void AttachToControllerRb(Rigidbody rb)
	{
		var snapTransform = rb.gameObject.transform;
		transform.position       = snapTransform.position;
		transform.forward        = snapTransform.forward;
		fixedJoint.connectedBody = rb;
		grabInteractable.enabled = false;
	}

	public void SetShootOn(InputActionReference shootAction)
	{
		shootAction.action.performed += ctx => StartShooting();
		shootAction.action.canceled += ctx => StopShooting();
	}

	private IEnumerator FireCoroutine()
	{
		while (true)
		{
			var bubble = Instantiate(bubblePrefab, bubbleSpawnPoint);
			bubble.velocity = transform.forward * bubbleSpeed;
			bubble.transform.SetParent(null);

			_currentAmmoInMagazine--;
			_lastShotTime = Time.time;
			
			UpdateAmmoText();
			if (_currentAmmoInMagazine == 0)
			{
				_canShoot = false;
				animator.SetBool("IsReloading", true);
				yield break;
			}

			yield return new WaitForSeconds(fireDelay);
		}
	}

	// will be called in the end of reload animation
	private void FinishReload()
	{
		_currentAmmoInMagazine =  Mathf.Min(magazineCapacity, totalAmmoAmount);
		totalAmmoAmount        -= _currentAmmoInMagazine;

		animator.SetBool("IsReloading", false);
		UpdateAmmoText();

		_canShoot = true;
	}
	private void UpdateAmmoText() => ammoText.SetText($"{_currentAmmoInMagazine} / {totalAmmoAmount}");
	
}