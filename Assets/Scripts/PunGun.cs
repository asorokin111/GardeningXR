using System;
using System.Collections;
using System.Collections.Generic;
using Nrjwolf.Tools.AttachAttributes;
using UnityEngine;
using UnityEngine.Events;

public class PunGun : MonoBehaviour
{
	[Header("Gun properties")]
	[SerializeField] private float FireDelay;
	[SerializeField] private int        TotalAmmoAmount;
	[SerializeField] private int        MagazineCapacity;
	[SerializeField] private UnityEvent OnAmmoAmountChanged;
	[GetComponent] [SerializeField] private Animator animator;

	[Header("Bullet properties")] 
	[SerializeField] private Rigidbody bulletPrefab;
	[SerializeField] private Transform bulletSpawnPoint;
	[SerializeField] private float     bulletSpeed;
	
	public int CurrentTotalAmmo      { get; private set; }
	public int CurrentAmmoInMagazine { get; private set; }

	private IEnumerator _shootCoroutine;
	private float _lastShotTime;
	private bool _canShoot = true;

	private void Start()
	{
		CurrentTotalAmmo      = TotalAmmoAmount;
		CurrentAmmoInMagazine = MagazineCapacity;

		_shootCoroutine = ShootCoroutine();
	}

	// will be called by player
	public void Shoot()
	{
		
	}

	private IEnumerator ShootCoroutine()
	{
		while (true)
		{
			FireBullet();
			
			CurrentAmmoInMagazine--;
			OnAmmoAmountChanged.Invoke();
			
			// used in case if player will try to shoot faster than FireDelay
			_lastShotTime = Time.time;
			
			if (CurrentAmmoInMagazine == 0)
			{
				_canShoot = false;
				animator.SetTrigger("Reload");
				StopCoroutine(_shootCoroutine);
			}
			
			yield return new WaitForSeconds(FireDelay);
		}
	}

	// will be called in the end of reload animation
	private void FinishReloading()
	{
		// use min in case when amount of left total ammo is less than magazine capacity
		CurrentAmmoInMagazine =  Mathf.Min(MagazineCapacity, CurrentTotalAmmo);
		CurrentTotalAmmo      -= CurrentAmmoInMagazine;
		OnAmmoAmountChanged.Invoke();
	}

	private void FireBullet()
	{
		var bulletRb = Instantiate(bulletPrefab, bulletSpawnPoint);
		bulletRb.velocity = bulletSpawnPoint.forward * bulletSpeed;
		bulletRb.transform.SetParent(null);
	}
}
