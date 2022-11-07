using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Room : MonoBehaviour
{
	public enum RoomType
	{
		LR,
		LRB,
		LRT,
		LRTB
	}

	[SerializeField] public RoomType roomType;

	public void DestroyRoom()
	{
		Destroy(gameObject);
	}
}
