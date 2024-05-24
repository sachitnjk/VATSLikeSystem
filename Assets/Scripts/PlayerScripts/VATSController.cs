using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class VATSController : MonoBehaviour
{
	private PlayerInput playerInput;
	private InputAction vatsAction;
	private InputAction lookAction;

	private Camera playerCam;

	private Collider previousSelectedCollider;
	private Collider currentSelectedCollider;

	private float originalTimeScale;
	private float vatsDistance;

	private void Start()
	{
		playerInput = InputProvider.GetPlayerInput();
		vatsAction = playerInput.actions["VATSMode"];
		lookAction = playerInput.actions["Look"];

		playerCam = GetComponentInChildren<Camera>();

		originalTimeScale = Time.timeScale;
		
		Cursor.lockState= CursorLockMode.Locked;
	}

	private void Update()
	{
		if(vatsAction.triggered)
		{
			ToggleVATS();
		}
		if(GameManager.Instance.vatsStatus)
		{
			VATSDistanceCalculation();
			EntityDetection();
		}
	}

	private void ToggleVATS()
	{
		if(Time.timeScale > 0.1f) 
		{
			GameManager.Instance.SetVatsStatus(true);
			Cursor.lockState = CursorLockMode.Confined;
			Time.timeScale = 0.1f;
		}
		else
		{
			Cursor.lockState= CursorLockMode.Locked;
			GameManager.Instance.SetVatsStatus(false);
			Time.timeScale = originalTimeScale;
		}
	}

	private void EntityDetection()
	{
		RaycastHit hit;

		Vector3 mosPosition = Mouse.current.position.ReadValue();

		Ray ray = playerCam.ScreenPointToRay(mosPosition);

		if(Physics.Raycast(ray, out hit, vatsDistance)) 
		{
			ColliderController entityColliderScript = hit.transform.gameObject.GetComponent<ColliderController>();
			//Debug.Log(hit.collider.gameObject.name);
			if (entityColliderScript != null)
			{
				Debug.Log(entityColliderScript.gameObject.name);
				entityColliderScript.SetVATSColliderStatus(true);

				//entityColliderScript.UpdateVATSDisplay();
				float visibilityScore = CalculateVisibilityScore(entityColliderScript);
				entityColliderScript.UpdateVATSDisplay(visibilityScore);
			}
		}
	}

	private void VATSDistanceCalculation()
	{
		vatsDistance = Inventory.Instance.GetCurrentWeaponReach();
	}

	private float CalculateVisibilityScore(ColliderController entityColliderScript)
	{
		float maxDistance = vatsDistance;
		float currentDistance = Vector3.Distance(playerCam.transform.position, entityColliderScript.transform.position);

		//Obstacle Checking
		RaycastHit hit;
		float obstaclePenalty = 0f;

		Vector3 direction = (entityColliderScript.transform.position - playerCam.transform.position).normalized;

		if(Physics.Raycast(playerCam.transform.position, direction, out hit, maxDistance))
		{
			if(hit.collider != GetComponent<ColliderController>())
			{
				obstaclePenalty = 0.5f;
			}
		}

		float visibilityScore = Mathf.Clamp01((maxDistance - currentDistance) / maxDistance - obstaclePenalty);
		return visibilityScore * 100;
	}
}
