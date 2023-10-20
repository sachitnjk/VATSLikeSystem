using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
	public static GameManager Instance { get; private set; }
	public bool vatsStatus {  get; private set; }

	private void Update()
	{
		Debug.Log(vatsStatus);
	}

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

	public void SetVatsStatus(bool currentStatus)
	{
		vatsStatus = currentStatus;
	}
}
