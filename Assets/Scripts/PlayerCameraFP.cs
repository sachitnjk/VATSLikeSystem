using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerCameraFP : MonoBehaviour
{
	[Header("Player References")]
	[SerializeField] private CharacterController playerCharController;

	[Header("Camera Attributes")]
	[Range(0.1f, 1f)] [SerializeField] private float camSensitivity;
	[SerializeField] private float camVerticalRotationClampAngle;

	private PlayerInput playerInput;
	private InputAction lookAction;

	private Vector2 lookInput;
	private Vector2 camRotation;

	private float verticalRotation = 0f;
	private float minVerticalCamRotation, maxVerticalCamRotation;

	private void Start()
	{
		playerInput = InputProvider.GetPlayerInput();

		if(playerInput != null )
		{
			lookAction = playerInput.actions["Look"];
		}

		//Temporary magic number will update soon
		camSensitivity /= 10;

		minVerticalCamRotation = -camVerticalRotationClampAngle;
		maxVerticalCamRotation = camVerticalRotationClampAngle;

	}

	private void Update()
	{
		PlayerCamControl();
	}

	private void PlayerCamControl()
	{
		lookInput = lookAction.ReadValue<Vector2>();

		camRotation.y += lookInput.x * camSensitivity;

		verticalRotation -= lookInput.y * camSensitivity;
		verticalRotation = Mathf.Clamp(verticalRotation, minVerticalCamRotation, maxVerticalCamRotation);

		transform.localRotation = Quaternion.Euler(verticalRotation, 0f, 0f);
		playerCharController.transform.rotation = Quaternion.Euler(0f, camRotation.y, 0f);
	}
}
