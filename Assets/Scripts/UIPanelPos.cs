using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class UIPanelPos : MonoBehaviour
{
	[SerializeField] private Transform vatsTransform;
	[SerializeField] private TextMeshProUGUI vatsTextBox;
	[SerializeField] private TextMeshProUGUI vatsTimesSelectedTextBox;

	private Camera mainCam;

	private int timesSelected = 0;

	private void Start()
	{
		mainCam = Camera.main;
	}

	private void FixedUpdate()
	{
		this.gameObject.transform.position = vatsTransform.position;
		this.gameObject.transform.rotation = vatsTransform.rotation;

		if(mainCam != null ) 
		{
			Vector3 cameraForward = mainCam.transform.forward;
			vatsTextBox.transform.rotation = Quaternion.LookRotation(cameraForward);
			vatsTimesSelectedTextBox.transform.rotation = Quaternion.LookRotation(cameraForward);
		}
	}

	public void SelectVATSPartUI()
	{
		timesSelected++;
		vatsTimesSelectedTextBox.text = timesSelected.ToString();
	}

	public void DeselectVATSPartUI()
	{
		timesSelected--;
		vatsTimesSelectedTextBox.text = timesSelected.ToString();
	}

	public void ResetVATSPartsUI()
	{
		timesSelected = 0;
		vatsTimesSelectedTextBox.text = timesSelected.ToString();
	}
}
