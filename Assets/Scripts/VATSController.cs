using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class VATSController : MonoBehaviour
{
	private PlayerInput playerInput;
	private InputAction vatsAction;

	private float originalTimeScale;

	private void Start()
	{
		playerInput = InputProvider.GetPlayerInput();
		vatsAction = playerInput.actions["VATSMode"];

		originalTimeScale = Time.timeScale;
	}

	private void Update()
	{
		if(vatsAction.triggered)
		{
			ToggleVATS();
		}
	}

	private void ToggleVATS()
	{
		if(Time.timeScale > 0.1f) 
		{
			Time.timeScale = 0.1f;
		}
		else
		{
			Time.timeScale = originalTimeScale;
		}
	}
}
