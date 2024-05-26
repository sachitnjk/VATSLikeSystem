using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UIPanelPos : MonoBehaviour
{
	[SerializeField] private Transform vatsTransform;

	private Camera playerCamera;

	private void Start()
	{
		playerCamera = Camera.main;
	}

	private void FixedUpdate()
	{
		if (playerCamera != null)
		{
			Debug.Log("going here");
			transform.LookAt(playerCamera.transform.position);
			transform.Rotate(0, 180, 0);
		}

		this.gameObject.transform.position = vatsTransform.position;
		this.gameObject.transform.rotation = vatsTransform.rotation;
	}
}
