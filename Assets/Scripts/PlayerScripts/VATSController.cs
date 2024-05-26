using Cinemachine;
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
	[SerializeField] private Transform vCamOriginalParent;
	[SerializeField] private GameObject primaryVCam;

	private Collider previousSelectedCollider;
	private Collider currentSelectedCollider;

	private ColliderController closestEntityScript;

	private float accuracyToEntity;
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
			if(closestEntityScript != null) 
			{
				closestEntityScript.UpdateVATSDisplay(accuracyToEntity);
			}
		}
	}

	private void ToggleVATS()
	{
		if(Time.timeScale > 0.1f) 
		{
			VATSDistanceCalculation();
			ActivateVATS();
			Cursor.lockState = CursorLockMode.Confined;
			Time.timeScale = 0.1f;
		}
		else
		{
			DeactivateVATS();
			Cursor.lockState= CursorLockMode.Locked;
			Time.timeScale = originalTimeScale;
		}
	}

	private void ActivateVATS()
	{
		Vector3 mousePosition = Mouse.current.position.ReadValue();
		Ray ray = playerCam.ScreenPointToRay(mousePosition);

		RaycastHit[] hits = Physics.SphereCastAll(ray, vatsDistance, vatsDistance);

		closestEntityScript = null;
		float closestDistance = Mathf.Infinity;

		foreach(RaycastHit hit in hits) 
		{
			ColliderController entityColliderScript = hit.transform.gameObject.GetComponent<ColliderController>();
			if(entityColliderScript != null)
			{
				float distance = Vector3.Distance(playerCam.transform.position, hit.transform.position);
				if(distance < closestDistance)
				{
					closestDistance = distance;
					closestEntityScript = entityColliderScript;
				}
			}
		}

		if(closestEntityScript != null ) 
		{
			closestEntityScript.SetVATSColliderStatus(true);
			primaryVCam.SetActive(false);
		}

	}

	private void DeactivateVATS()
	{
		if(closestEntityScript != null)
		{
			closestEntityScript.SetVATSColliderStatus(false);
			primaryVCam.SetActive(true);
		}
	}

	private void VATSDistanceCalculation()
	{
		vatsDistance = Inventory.Instance.GetCurrentWeaponReach();
	}
}
