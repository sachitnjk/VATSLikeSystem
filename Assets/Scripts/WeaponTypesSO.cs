using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "WeaponType")]
public class WeaponTypesSO : ScriptableObject
{
	public WeaponTypes weaponType;
	public float weaponReach;

	public enum WeaponTypes
	{
		Melee,
		ShortRange,
		LongRange
	}

}
