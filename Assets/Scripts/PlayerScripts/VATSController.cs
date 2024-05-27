using Cinemachine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class VATSController : MonoBehaviour
{
	private PlayerInput playerInput;
	private InputAction vatsAction;
	private InputAction shootAction;

	private Camera playerCam;
	[SerializeField] private Transform vCamOriginalParent;
	[SerializeField] private GameObject primaryVCam;
	[SerializeField] private GameObject secondaryVCam;

	private PlayerMovementFP playerMoveScript;
	private ColliderController closestEntityScript;

	private bool isVATSActive = false;
	private float originalTimeScale;
	private float vatsDistance;

	private void Start()
	{
		playerInput = InputProvider.GetPlayerInput();
		if(playerInput != null )
		{
			vatsAction = playerInput.actions["VATSMode"];
			shootAction = playerInput.actions["Shoot"];
		}

		playerMoveScript = this.gameObject.GetComponent<PlayerMovementFP>();
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

		if(isVATSActive)
		{
			CheckSelectedVATSPart();
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
		if (playerMoveScript != null)
		{
			playerMoveScript.MovementLock(true);
		}

		if(!isVATSActive)
		{
			isVATSActive = true;
		}

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

				secondaryVCam.transform.position = closestEntityScript.GetVATSCamTransform().position;
				secondaryVCam.transform.rotation = closestEntityScript.GetVATSCamTransform().rotation;
			}
		}

		if(closestEntityScript != null ) 
		{
			closestEntityScript.ShowHealthBar();
			closestEntityScript.SetVATSColliderStatus(true);
			primaryVCam.SetActive(false);
		}

	}

	private void DeactivateVATS()
	{
		if (playerMoveScript != null)
		{
			playerMoveScript.MovementLock(false);
		}

		if (isVATSActive)
		{
			isVATSActive = false;
		}

		if (closestEntityScript != null)
		{
			secondaryVCam.transform.position = primaryVCam.transform.position;
			secondaryVCam.transform.rotation = primaryVCam.transform.rotation;

			closestEntityScript.HideHealthBar();
			closestEntityScript.CleanUpVatsUi();
			closestEntityScript.SetVATSColliderStatus(false);
			primaryVCam.SetActive(true);
		}
	}

	private void VATSDistanceCalculation()
	{
		vatsDistance = Inventory.Instance.GetCurrentWeaponReach();
	}

	private void CheckSelectedVATSPart()
	{
		if(closestEntityScript != null)
		{
			Vector3 mousePosition = Mouse.current.position.ReadValue();
			Ray ray = playerCam.ScreenPointToRay(mousePosition);

			if(Physics.Raycast(ray, out RaycastHit hit))
			{
				Collider clickedCollider = hit.collider;
				List<Collider> entityPartColliders = closestEntityScript.GetCollidersList();

				foreach(Collider collider in entityPartColliders)
				{
					if(clickedCollider == collider)
					{
						if(shootAction.WasPerformedThisFrame())
						{
							closestEntityScript.SelectVATSOnPart(clickedCollider);
							Debug.Log(clickedCollider.gameObject.name);
						}
					}
				}
			}
		}
	}
}
