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
	private InputAction switchTargetRightAction;
	private InputAction switchTargetLeftAction;

	[Header("Serialized Object references")]
	[SerializeField] private Transform vCamOriginalParent;
	[SerializeField] private GameObject primaryVCam;
	[SerializeField] private GameObject secondaryVCam;

	[Header("Serialized Values")]
	[SerializeField] [Range(0.5f, 0.01f)] private float slowTimeScale;

	private Camera playerCam;
	private PlayerMovementFP playerMoveScript;
	private ColliderController closestEntityScript;

	private bool isVATSActive = false;
	private float originalTimeScale;
	private float vatsDistance;
	private List<ColliderController> detectedEntityCollidersList;

	private void Awake()
	{
		detectedEntityCollidersList = new List<ColliderController>();
	}

	private void Start()
	{
		playerInput = InputProvider.GetPlayerInput();
		if(playerInput != null )
		{
			vatsAction = playerInput.actions["VATSMode"];
			shootAction = playerInput.actions["Shoot"];
			switchTargetRightAction = playerInput.actions["SVT_Right"];
			switchTargetLeftAction = playerInput.actions["SVT_Left"];
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

		if( isVATSActive && closestEntityScript != null)
		{
			if (switchTargetRightAction.triggered)
			{
				CycleVATSTargetEntity(1);
			}
			else if(switchTargetLeftAction.triggered)
			{
				CycleVATSTargetEntity(-1);
			}
		}
	}

	private void ToggleVATS()
	{
		if(Time.timeScale > slowTimeScale) 
		{
			VATSDistanceCalculation();
			ActivateVATS();
			Cursor.lockState = CursorLockMode.Confined;
			Time.timeScale = slowTimeScale;
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
				detectedEntityCollidersList.Add(entityColliderScript);

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

		if(detectedEntityCollidersList != null)
		{
			detectedEntityCollidersList.Clear();
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
						}
					}
				}
			}
		}
	}

	private void CycleVATSTargetEntity(int direction)
	{
		int currentIndex = detectedEntityCollidersList.IndexOf(closestEntityScript);
		int nextIndex = (currentIndex + direction) % detectedEntityCollidersList.Count;

		if(nextIndex == currentIndex ) 
		{
			return;
		}

		if(nextIndex < 0)
		{
			nextIndex = detectedEntityCollidersList.Count - 1;
		}

		closestEntityScript.HideHealthBar();
		closestEntityScript.CleanUpVatsUi();
		closestEntityScript.SetVATSColliderStatus(false);

		//Setting new updated closest entity 
		closestEntityScript = detectedEntityCollidersList[nextIndex];
		closestEntityScript.ShowHealthBar();
		closestEntityScript.SetVATSColliderStatus(true);

		//current closest entity cleanUp
		secondaryVCam.transform.position = closestEntityScript.GetVATSCamTransform().position;
		secondaryVCam.transform.rotation = closestEntityScript.GetVATSCamTransform().rotation;
	}
}
