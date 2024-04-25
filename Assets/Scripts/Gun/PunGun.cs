using System.Collections;
using Nrjwolf.Tools.AttachAttributes;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.Pool;

public class PunGun : MonoBehaviour
{
	
	[Header("Gun properties")]
	[SerializeField] private float FireDelay;
	[SerializeField] private int        TotalAmmoAmount;
	[SerializeField] private int        MagazineCapacity;
	[SerializeField] private UnityEvent OnAmmoAmountChanged;
	[GetComponent] [SerializeField] private Animator animator;

	[Header("Bullet properties")] 
	[SerializeField] private Bubble bulletPrefab;
	[SerializeField] private Transform bulletSpawnPoint;
	[SerializeField] private float     bulletSpeed;
	
	public int CurrentTotalAmmo      { get; private set; }
	public int CurrentAmmoInMagazine { get; private set; }

	private                 IEnumerator _shootCoroutine;
	private                 float       _lastShotTime;
	private                 bool        _isReloading;
	private static readonly int         Reload       = Animator.StringToHash("Reload");

	private ObjectPool<Bubble> _bubblePool;
	
	private void Start()
	{
		CurrentTotalAmmo      = TotalAmmoAmount;
		CurrentAmmoInMagazine = MagazineCapacity;
		
		_bubblePool = new ObjectPool<Bubble>(
			() => Instantiate(bulletPrefab),
			bullet => bullet.gameObject.SetActive(true),
			bullet => bullet.gameObject.SetActive(false),
			bullet =>Destroy(bullet.gameObject),
			false, 10, 20
		);

		_shootCoroutine = ShootCoroutine();
	}

	// will be called by player
	public void StartShooting()
	{
		// if player tries to shoot faster than FireDelay or is reloading 
		if (_lastShotTime + FireDelay > Time.time || _isReloading) return;
		StartCoroutine(_shootCoroutine);
	}

	// will be called by player
	public void StopShooting()
	{
		StopCoroutine(_shootCoroutine);
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
				_isReloading = true;
				animator.SetTrigger(Reload);
				StopShooting();
			}
			
			yield return new WaitForSeconds(FireDelay);
		}
	}

	// will be called in the end of reload animation
	private void FinishReloading()
	{
		_isReloading = false;
		// use min in case when amount of left total ammo is less than magazine capacity
		CurrentAmmoInMagazine =  Mathf.Min(MagazineCapacity, CurrentTotalAmmo);
		CurrentTotalAmmo      -= CurrentAmmoInMagazine;
		OnAmmoAmountChanged.Invoke();
	}

	private void FireBullet()
	{
		var bullet = _bubblePool.Get();
		bullet.Init(KillBubble);
		bullet.transform.position = bulletSpawnPoint.position;
		bullet.SetVelocity(bulletSpawnPoint.forward * bulletSpeed);
		bullet.transform.SetParent(null);
	}
	
	private void KillBubble(Bubble bubble) => _bubblePool.Release(bubble);
}
