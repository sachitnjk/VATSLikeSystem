using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyMove : MonoBehaviour
{
	[SerializeField] private Transform walkToPoint;
	[SerializeField] private float speed = 5f;


	private Vector3 originalPosition;
	private bool movingToWalkPoint = true;

	private void Start()
	{
		originalPosition = transform.position;
	}

	private void Update()
	{
		Walk();
	}

	private void Walk()
	{
		if(movingToWalkPoint) 
		{
			transform.position = Vector3.MoveTowards(transform.position, walkToPoint.position, speed* Time.deltaTime);
			if(Vector3.Distance(transform.position, walkToPoint.position) <= 0.5f)
			{
				movingToWalkPoint = false;
			}
		}
		else
		{
			transform.position = Vector3.MoveTowards(transform.position, originalPosition, speed * Time.deltaTime);

			if(Vector3.Distance(transform.position, originalPosition) <= 0.5f)
			{
				movingToWalkPoint = true;
			}
		}
	}
}
