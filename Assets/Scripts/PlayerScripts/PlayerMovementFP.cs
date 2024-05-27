using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.InputSystem;

[RequireComponent(typeof(CharacterController))]
public class PlayerMovementFP : MonoBehaviour
{
	private PlayerInput playerInput;
	private InputAction moveAction;

	private Animator playerAnimator;
	private CharacterController playerCharController;

	private bool movementLockStatus = false;

	private Vector2 moveInput;
	private Vector3 moveDirection;

	[SerializeField] private float moveSpeed;

	private void Start()
	{
		playerInput = InputProvider.GetPlayerInput();

		if(playerInput != null ) 
		{
			moveAction = playerInput.actions["Move"];
		}

		playerCharController = GetComponent<CharacterController>();
		playerAnimator = GetComponentInChildren<Animator>();
	}

	private void Update()
	{
		if(!movementLockStatus)
		{
			Move();
		}
	}

	private void Move()
	{
		moveInput = moveAction.ReadValue<Vector2>();
		
		moveDirection = transform.forward * moveInput.y + transform.right * moveInput.x;

		playerCharController.Move(moveDirection.normalized * moveSpeed * Time.deltaTime);

		if(moveDirection.magnitude > 0 ) 
		{
			playerAnimator.SetBool("isWalking", true);
		}
		else
		{
			playerAnimator.SetBool("isWalking", false);
		}

	}

	public void MovementLock(bool lockStatus)
	{
		movementLockStatus = lockStatus;
	}
}
