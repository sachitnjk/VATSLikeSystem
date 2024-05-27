using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class HealthController : MonoBehaviour
{
	[SerializeField] private float maxHealth;
	[field: SerializeField] public Slider healthSlider { get; private set; }
	private float deathHealth = 0;
	public float currentHealth { get; private set; }

	private void Start()
	{
		currentHealth = maxHealth;
		healthSlider.value = currentHealth;
	}

	public void DecreaseHealth()
	{
		if(currentHealth > deathHealth)
		{
			currentHealth--;
		}
		healthSlider.value = currentHealth;
	}
}
