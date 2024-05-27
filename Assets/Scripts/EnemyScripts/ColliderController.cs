using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class ColliderController : MonoBehaviour
{
	[SerializeField] private HealthController healthController;
	[SerializeField] private Transform vatsCamTransform;
	[SerializeField] private List<ColliderToPanel> colliderToPanelList;

	private List<Collider> partColliders;
	private Collider mainCollider;

	[System.Serializable]
	public struct ColliderToPanel
	{
		public Collider partCollider;
		public Transform partVATSPanel;
		public TextMeshProUGUI vatsTextBox;
	}

	private void Start()
	{
		mainCollider = GetComponent<Collider>();
		SetVATSColliderStatus(false);

		if(healthController != null) 
		{
			HideHealthBar();
		}

		partColliders = new List<Collider>();
		foreach (ColliderToPanel colliderToCanvas in colliderToPanelList)
		{
			partColliders.Add(colliderToCanvas.partCollider);
		}
	}

	public void SetVATSColliderStatus(bool status)
	{
		mainCollider.enabled = !status;
		foreach (ColliderToPanel colliderToPanel in colliderToPanelList) 
		{
			colliderToPanel.partCollider.enabled = status;
			colliderToPanel.partVATSPanel.gameObject.SetActive(status);
		}
	}

	//public void UpdateVATSDisplay(float vatsAccuracy)
	//{
	//	foreach(ColliderToCanvas colliderToCanvas in colliderToCanvasList)
	//	{
	//		colliderToCanvas.vatsTextBox.text = vatsAccuracy.ToString("F2");
	//		Debug.Log(colliderToCanvas.vatsTextBox.text.ToString());
	//	}
	//}

	public void ShowHealthBar()
	{
		healthController.healthSlider.gameObject.SetActive(true);
	}

	public void HideHealthBar()
	{
		healthController.healthSlider.gameObject.SetActive(false);
	}

	public void CleanUpVatsUi()
	{
		foreach(ColliderToPanel colliderToPanel in colliderToPanelList)
		{
			UIPanelPos uiPanelPosScript = colliderToPanel.partVATSPanel.GetComponent<UIPanelPos>();
			uiPanelPosScript.ResetVATSPartsUI();
		}
	}

	public void SelectVATSOnPart(Collider selectedCollider)
	{
		foreach(ColliderToPanel colliderToPanel in colliderToPanelList)
		{
			if(selectedCollider == colliderToPanel.partCollider)
			{
				UIPanelPos uiPanelScript = colliderToPanel.partVATSPanel.GetComponent<UIPanelPos>();
				if(uiPanelScript != null)
				{
					uiPanelScript.SelectVATSPartUI();
				}
			}
		}
	}

	public Transform GetVATSCamTransform()
	{
		if(vatsCamTransform != null)
		{
			return vatsCamTransform.transform;
		}
		return null;
	}
	public List<Collider> GetCollidersList()
	{
		return partColliders;
	}
}
