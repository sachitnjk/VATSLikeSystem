using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UIFacePlayer : MonoBehaviour
{
	private Camera playerCamera;

	private void Start()
	{
		playerCamera = Camera.main;
	}

	private void Update()
	{
		if(playerCamera != null) 
		{
			Debug.Log("going here");
			transform.LookAt(playerCamera.transform.position);
			transform.Rotate(0, 180, 0);
		}
	}
}
