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
		NewStart();
		// build main path
		while(!stopLevelGeneration)
			GeneratePath();
		GenerateRemainingRooms();
		// place player and goal
		AddPlayerAndGoal();
	}

	private void NewStart()
	{
		int start = Random.Range(0, 5);
		transform.position = startPositions[start].position;
		playerStart = startPositions[start];

		Instantiate(dungeonRooms[1], transform.position, Quaternion.identity);

		//pick a direction to move
		// 0 - down, 1 - left, 2 - right
		if (transform.position.x - roomSize < minX)
			moveDirection = 2;
		else
			moveDirection = 1;
		//moveDirection = Random.Range(1, 3); // move left or right
	}

	private void GeneratePath()
	{
		switch (moveDirection)
		{
			case 0: // down
				if (transform.position.y - roomSize < minY)
				{
					//stop level generation
					playerEnd = transform;
					stopLevelGeneration = true;
				}
				else
				{
					transform.position = new Vector3(transform.position.x, transform.position.y - roomSize);

					if (Random.Range(0, 2) == 1) // coin toss
					{
						Instantiate(dungeonRooms[4], transform.position, Quaternion.identity);
						moveDirection = Random.Range(1, 3);
					}
					else
					{
						Instantiate(dungeonRooms[4], transform.position, Quaternion.identity);
						moveDirection = Random.Range(0, 3);
					}
				}
				break;
			case 1: // left
				if (transform.position.x - roomSize < minX)
				{
					moveDirection = 0;
				}
				else
				{
					transform.position = new Vector3(transform.position.x - roomSize, transform.position.y);

					if (CoinToss())
					{
						moveDirection = 0; // moving down, spawn a room with a bottom opening

						if (CoinToss())
							Instantiate(dungeonRooms[2], transform.position, Quaternion.identity);
						else
							Instantiate(dungeonRooms[4], transform.position, Quaternion.identity);
					}
					else
					{
						moveDirection = 1; // moving left, spawn any room

						if (transform.position.x - roomSize < minX)
						{
							if (CoinToss())
								Instantiate(dungeonRooms[2], transform.position, Quaternion.identity);
							else
								Instantiate(dungeonRooms[4], transform.position, Quaternion.identity);
						}
						else
						{
							int room = Random.Range(1, 5);
							Instantiate(dungeonRooms[room], transform.position, Quaternion.identity);
						}
					}
				}
				break;
			case 2: // right
				if (transform.position.x + roomSize > maxX)
				{
					moveDirection = 0;
				}
				else
				{
					transform.position = new Vector3(transform.position.x + roomSize, transform.position.y);

					if (CoinToss())
					{
						moveDirection = 0; // moving down, spawn a room with a bottom opening

						if (CoinToss())
							Instantiate(dungeonRooms[2], transform.position, Quaternion.identity);
						else
							Instantiate(dungeonRooms[4], transform.position, Quaternion.identity);
					}
					else
					{
						moveDirection = 2; // moving right, spawn any room

						if (transform.position.x + roomSize > maxX)
						{
							if (CoinToss())
								Instantiate(dungeonRooms[2], transform.position, Quaternion.identity);
							else
								Instantiate(dungeonRooms[4], transform.position, Quaternion.identity);
						}
						else
						{
							int room = Random.Range(1, 5);
							Instantiate(dungeonRooms[room], transform.position, Quaternion.identity);
						}
					}
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

	private bool CoinToss()
	{
		if (Random.Range(0, 2) == 1)
			return true;
		else
			return false;
	}
}
