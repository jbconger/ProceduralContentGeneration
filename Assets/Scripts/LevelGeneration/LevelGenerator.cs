using Cinemachine;
using UnityEngine;

public class LevelGenerator : MonoBehaviour
{
	#region Fields
	[Header("Level Components")]
    [SerializeField] private Transform[] startPositions;
	[SerializeField] private Transform[] allRoomPositions;
	[SerializeField] private GameObject[] dungeonRooms;

	[Header("Spawning Parameters")]
	[SerializeField] private float maxX;
	[SerializeField] private float minX;
	[SerializeField] private float minY;

	private int moveDirection;
	private int downCounter;
	private float roomSize = 10.0f;
	private bool stopLevelGeneration = false;
	[SerializeField] public LayerMask room;

	//player and objectives
	[Header("Player")]
	[SerializeField] private GameObject player;
	[SerializeField] private CinemachineVirtualCamera playerCamera;
	[SerializeField] private GameObject goal;
	[SerializeField] private GoalMenu goalMenu;
	private Transform playerStart;
	private Transform playerEnd;

	#endregion

	private void Start()
	{
		BuildDungeon();
	}


	private void BuildDungeon()
	{
		// build main path
		BuildMainPath();
		// fill remaining rooms
		GenerateRemainingRooms();
		// place player and goal
		AddPlayerAndGoal();
	}

	private void BuildMainPath()
	{
		PlaceStartingRoom();

		while (!stopLevelGeneration)
			MoveAndPlaceRooms();
	}
	private void PlaceStartingRoom()
	{
		int randomStartIndex = Random.Range(0, 4);
		transform.position = startPositions[randomStartIndex].position;
		playerStart = startPositions[randomStartIndex];
		Instantiate(dungeonRooms[randomStartIndex], transform.position, Quaternion.identity, this.transform);

		moveDirection = Random.Range(0, 4);
	}

	private void MoveAndPlaceRooms()
	{
		switch (moveDirection)
		{
			case 1: // move right
			case 2:
				if (transform.position.x < maxX)
				{
					downCounter = 0;

					transform.position = new Vector2(transform.position.x + roomSize, transform.position.y);

					int randomRoomIndex = Random.Range(0, dungeonRooms.Length); // grab a room to spawn
					Instantiate(dungeonRooms[randomRoomIndex], transform.position, Quaternion.identity);

					moveDirection = Random.Range(0, 3); // continue down or right
				}
				else
					moveDirection = 0;
				break;
			case 3: // move left
			case 4:
				if (transform.position.x > minX)
				{
					downCounter = 0;

					transform.position = new Vector2(transform.position.x - roomSize, transform.position.y);

					int randomRoomIndex = Random.Range(0, dungeonRooms.Length); // grab a room to spawn
					Instantiate(dungeonRooms[randomRoomIndex], transform.position, Quaternion.identity);

					moveDirection = Random.Range(3, 6); // continue down or left
				}
				else
					moveDirection = 0;
				break;
			default: // move down
				downCounter++;

				if (transform.position.y > minY)
				{
					Collider2D previousRoom = Physics2D.OverlapCircle(transform.position, 1, room); // check for opening in previous room
					if (previousRoom && (previousRoom.GetComponent<Room>().roomType == Room.RoomType.LR || previousRoom.GetComponent<Room>().roomType == Room.RoomType.LRT))
					{
						// replace room if it does not have an opening
						if (downCounter >= 2)
						{
							previousRoom.GetComponent<Room>().DestroyRoom();
							Instantiate(dungeonRooms[3], previousRoom.gameObject.transform.position, Quaternion.identity);
						}
						else
						{
							previousRoom.GetComponent<Room>().DestroyRoom();

							if (Random.Range(0, 10) % 2 == 1)
								Instantiate(dungeonRooms[1], transform.position, Quaternion.identity);
							else
								Instantiate(dungeonRooms[3], transform.position, Quaternion.identity);
						}
					}

					transform.position = new Vector2(transform.position.x, transform.position.y - roomSize);

					int randomRoomIndex = Random.Range(2, 4);
					Instantiate(dungeonRooms[randomRoomIndex], transform.position, Quaternion.identity);

					moveDirection = Random.Range(0, 5); // continue right, down, or left
				}
				else
				{
					playerEnd = transform;
					stopLevelGeneration = true;
				}
				break;
		}
	}

	private void GenerateRemainingRooms()
	{
		foreach(Transform roomSlot in allRoomPositions)
		{
			Collider2D roomCheck = Physics2D.OverlapCircle(roomSlot.position, 1, room);

			if (roomCheck == null)
			{
				Instantiate(dungeonRooms[Random.Range(0, dungeonRooms.Length)], roomSlot.position, Quaternion.identity);
			}
		}
	}

	private void AddPlayerAndGoal()
	{
		playerCamera.Follow = Instantiate(player, playerStart.position, Quaternion.identity).transform;

		Instantiate(goal, playerEnd.position, Quaternion.identity);
		Goal.onGoalTrigger.AddListener(goalMenu.DisplayGoalMenu);
	}
}
