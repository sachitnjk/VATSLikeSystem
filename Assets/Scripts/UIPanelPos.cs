using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class UIPanelPos : MonoBehaviour
{
	[SerializeField] private Transform vatsTransform;
	[SerializeField] private TextMeshProUGUI vatsTextBox;

	private void FixedUpdate()
	{
		this.gameObject.transform.position = vatsTransform.position;
		this.gameObject.transform.rotation = vatsTransform.rotation;
	}
}
