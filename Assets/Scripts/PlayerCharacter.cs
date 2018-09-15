using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerCharacter : MonoBehaviour
{
	public static PlayerCharacter Instance;

	[SerializeField]
	private float speed = 5f;

	private void Awake()
	{
		Instance = this;
	}

	public void MoveToTarget(Vector3 pos)
	{
		Debug.Log("move");
		transform.position = pos;
	}
}