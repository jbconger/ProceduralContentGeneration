using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour
{
	#region Player Fields
	// Player Components
	[HideInInspector] public Rigidbody2D rb2D;
	[HideInInspector] public Animator anim;
	[HideInInspector] public SpriteRenderer sprite;
	[HideInInspector] public CapsuleCollider2D playerCollider;
	[HideInInspector] public PlayerAudio playerAudio;

	// States
	[HideInInspector] public StateMachine stateMachine;
	[HideInInspector] public GroundState groundState;
	[HideInInspector] public MoveState moveState;
	[HideInInspector] public CrouchState crouchState;
	[HideInInspector] public JumpState jumpState;

	// Inputs
	public struct Inputs
	{
		public int rawX;
		public int rawY;
		public float x;
		public float y;
	}

	[HideInInspector] public Inputs inputs;
	[HideInInspector] public bool isFacingLeft;

	// Ability Indicators
	[Header("Abilities")]
	[SerializeField] public bool canCrouch = false;
	[SerializeField] public bool canDoubleJump = false;

	// Movement Parameters
	[Header("Checks")]
	[SerializeField] public LayerMask groundMask;
	[SerializeField] public float groundOffset = -1;
	[SerializeField] public float groundRadius = 0.2f;
	[HideInInspector] public bool isGrounded;

	[Header("Movement")]
	[SerializeField] public float moveSpeed = 10;
	[SerializeField] public float acceleration = 4;
	[SerializeField] public float moveLerpSpeed = 100;

	[Header("Crouch")]
	[SerializeField] public Vector2 crouchSize = new Vector2(0.8f, 0.8f);
	[SerializeField] public float crouchOffset = -0.5f;
	[SerializeField] [Range(0,1)] public float crouchMoveSpeed = 0.3f;
	[HideInInspector] public Vector2 playerSize;
	[HideInInspector] public Vector2 playerOffset;

	[Header("Jump")]
	[SerializeField] public float jumpForce = 15;
	[SerializeField] public float fallMultiplier = 9;
	[SerializeField] public float jumpVelocityFalloff = 12;

	#endregion

	void Start()
	{
		// grab components
		rb2D = GetComponent<Rigidbody2D>();
		anim = GetComponent<Animator>();
		sprite = GetComponent<SpriteRenderer>();
		playerCollider = GetComponent<CapsuleCollider2D>();
		playerAudio = GetComponent<PlayerAudio>();

		CapsuleCollider2D collider = GetComponent<CapsuleCollider2D>();
		playerSize = collider.size;
		playerOffset = collider.offset;

		// create states
		stateMachine = new StateMachine();

		groundState = new GroundState(this);
		moveState = new MoveState(this);
		crouchState = new CrouchState(this);
		jumpState = new JumpState(this);

		// start state machine
		stateMachine.Initialize(moveState);
	}

	void Update()
	{
		CheckInput();
		stateMachine.RunState(groundState);
		stateMachine.RunState();
	}

	private void CheckInput()
	{
		inputs.rawX = (int)Input.GetAxisRaw("Horizontal");
		inputs.rawY = (int)Input.GetAxisRaw("Vertical");
		inputs.x = Input.GetAxis("Horizontal");
		inputs.y = Input.GetAxis("Vertical");

		isFacingLeft = inputs.rawX != 1 && (inputs.rawX == -1 || isFacingLeft);
		sprite.flipX = isFacingLeft;
	}
}
