using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Inventory : MonoBehaviour
{
	public static Inventory Instance { get; private set; }
	public WeaponTypesSO.WeaponTypes currentlyEquipped;

	public List<WeaponTypesSO> weapons;


	private void Awake()
	{
		if (Instance == null)
		{
			Instance = this;
		}
		else
		{
			Destroy(Instance);
		}
	}

	public float GetCurrentWeaponReach()
	{
		foreach(WeaponTypesSO weapon in weapons) 
		{
			if(weapon.weaponType == currentlyEquipped)
			{
				return weapon.weaponReach;
			}
		}
		return 0f;
	}

}

