using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerCharacter : MonoBehaviour
{
	public static PlayerCharacter Instance;

	[SerializeField]
	private float speed = 5f;
	[SerializeField]
	private float heightAboveGround = .1f;
	private bool isMoving;
	private Vector3 target;
	private Vector3 targetVector;
	[SerializeField]
	private float stopDistance = .1f;

	private void Awake()
	{
		Instance = this;
	}

	private void Update()
	{
		if (isMoving)
		{
			RaycastHit hit;
			Physics.Raycast(transform.position, Vector3.down, out hit, 1f);

			if (null != hit.collider)
				transform.Translate(targetVector * speed * Time.deltaTime);

			if (Vector3.Distance(transform.position, target) <= stopDistance)
				isMoving = false;
		}
	}

	public void MoveToTarget(Vector3 pos)
	{
		Debug.Log("move");
		target = pos;
		targetVector = pos - transform.position;
		isMoving = true;
		//transform.position = pos;
	}
}