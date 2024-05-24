using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class ColliderController : MonoBehaviour
{
	//[SerializeField] private List<Collider> partColliders;
	[SerializeField] private List<ColliderToCanvas> colliderToCanvasList;

	private List<Collider> partColliders;
	private Collider mainCollider;

	[System.Serializable]
	public struct ColliderToCanvas
	{
		public Collider partCollider;
		public Canvas partVATSCanvas;
		public TextMeshProUGUI vatsTextBox;
	}

	private void Start()
	{
		mainCollider = GetComponent<Collider>();
		SetVATSColliderStatus(false);

		partColliders = new List<Collider>();
		foreach(ColliderToCanvas colliderToCanvas in colliderToCanvasList) 
		{
			partColliders.Add(colliderToCanvas.partCollider);
		}
	}

	private void Update()
	{
		if (!GameManager.Instance.vatsStatus)
		{
			SetVATSColliderStatus(false);
		}
	}

	public void SetVATSColliderStatus(bool status)
	{
		mainCollider.enabled = !status;
		foreach (ColliderToCanvas colliderToCanvas in colliderToCanvasList) 
		{
			colliderToCanvas.partCollider.enabled = status;
			colliderToCanvas.partVATSCanvas.enabled = status;
		}
	}

	public void UpdateVATSDisplay(float vatsAccuracy)
	{
		foreach(ColliderToCanvas colliderToCanvas in colliderToCanvasList)
		{
			colliderToCanvas.vatsTextBox.text = vatsAccuracy.ToString("F2");
			Debug.Log(colliderToCanvas.vatsTextBox.text.ToString());
		}
	}

	public List<Collider> GetCollidersList()
	{
		return partColliders;
	}
}
