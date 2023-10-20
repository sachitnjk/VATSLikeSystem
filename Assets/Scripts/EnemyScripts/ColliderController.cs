using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ColliderController : MonoBehaviour
{
	[SerializeField] private List<Collider> partColliders;
	private Collider mainCollider;

	private void Start()
	{
		mainCollider = GetComponent<Collider>();
		SetColliderStatus(false);
	}

	private void Update()
	{
		if(GameManager.Instance.vatsStatus)
		{
			SetColliderStatus(true);
		}
		else if(!GameManager.Instance.vatsStatus)
		{
			SetColliderStatus(false);
		}
	}

	private void SetColliderStatus(bool status)
	{
		foreach(Collider collider in partColliders)
		{
			mainCollider.enabled= !status;
			collider.enabled = status;
		}
	}

	public List<Collider> GetCollidersList()
	{
		return partColliders;
	}
}
