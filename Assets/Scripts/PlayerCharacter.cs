using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerCharacter : MonoBehaviour
{
	public static PlayerCharacter Instance;

	private void Awake()
	{
		Instance = this;
	}

	public void MoveToTarget(Vector3 pos)
	{
		//Debug.Log("move");
		Vector3 vectorRef = Vector3.zero;
		transform.position = Vector3.SmoothDamp(transform.position, pos, ref vectorRef, 0.15f);
	}
}