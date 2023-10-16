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

	//private Animator playerAnimator;
	private CharacterController playerCharController;

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
		//playerAnimator = GetComponentInChildren<Animator>();
	}

	private void Update()
	{
		Move();
	}

	private void Move()
	{
		moveInput = moveAction.ReadValue<Vector2>();
		
		moveDirection = transform.forward * moveInput.y + transform.right * moveInput.x;

		playerCharController.Move(moveDirection.normalized * moveSpeed * Time.deltaTime);

		//Add animator functionality here if needed
	}
}
