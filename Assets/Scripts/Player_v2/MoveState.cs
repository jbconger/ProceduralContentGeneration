using UnityEngine;

public class MoveState : State
{
	private bool crouch, jump;

    public MoveState(Player player) : base(player) { }

	public override void Enter()
	{
		base.Enter();
		crouch = false; jump = false;
	}

	public override void Exit()
	{
		base.Exit();
	}

	public override void HandleInput()
	{
		base.HandleInput();

		if ((Input.GetKeyDown(KeyCode.DownArrow) || Input.GetKeyDown(KeyCode.S)) && player.isGrounded && player.canCrouch)
			crouch = true;
		else if ((Input.GetKeyDown(KeyCode.Space) || Input.GetKeyDown(KeyCode.Z)) && player.isGrounded)
			jump = true;
	}

	public override void StateUpdate()
	{
		base.StateUpdate();
		Moving();

		if (crouch)
			player.stateMachine.ChangeState(player.crouchState);
		else if (jump)
			player.stateMachine.ChangeState(player.jumpState);
	}

	private void Moving()
	{
		float newAcceleration = player.isGrounded ? player.acceleration : player.acceleration * 0.5f;

		if (Input.GetKey(KeyCode.LeftArrow) || Input.GetKey(KeyCode.A))
		{
			if (player.rb2D.velocity.x > 0)
				player.inputs.x = 0; // immediately stops and turns player

			player.inputs.x = Mathf.MoveTowards(player.inputs.x, -1, newAcceleration * Time.deltaTime);
		}
		else if (Input.GetKey(KeyCode.RightArrow) || Input.GetKey(KeyCode.D))
		{
			if (player.rb2D.velocity.x < 0)
				player.inputs.x = 0; // immediately stops and turns player

			player.inputs.x = Mathf.MoveTowards(player.inputs.x, 1, newAcceleration * Time.deltaTime);
		}
		else
		{
			player.inputs.x = Mathf.MoveTowards(player.inputs.x, 0, newAcceleration * 2 * Time.deltaTime);
		}

		Vector2 velocity = new Vector2(player.inputs.x * player.moveSpeed, player.rb2D.velocity.y);
		player.rb2D.velocity = Vector2.MoveTowards(player.rb2D.velocity, velocity, player.moveLerpSpeed * Time.deltaTime);

		// set walking animation
		player.anim.SetBool("Move", player.inputs.rawX != 0 && player.isGrounded);
	}
}
