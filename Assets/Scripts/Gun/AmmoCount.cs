using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class AmmoCount : MonoBehaviour
{
	[SerializeField] private TMP_Text ammoText;

	public void UpdateAmmo(PunGun punGun)
	{
		ammoText.SetText($"{punGun.CurrentAmmoInMagazine} / {punGun.CurrentTotalAmmo}");
	}
}
