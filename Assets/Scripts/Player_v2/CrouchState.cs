using UnityEngine;

public class CrouchState : State
{
    private bool uncrouch;
    
    public CrouchState(Player player) : base(player) { }

	public override void Enter()
	{
		base.Enter();
		uncrouch = false;
		Crouch();
	}

	public override void Exit()
	{
		base.Exit();
		Uncrouch();
	}

	public override void HandleInput()
	{
		base.HandleInput();

		if (Input.GetKeyUp(KeyCode.S) || Input.GetKeyUp(KeyCode.DownArrow))
			uncrouch = true;
	}

	public override void StateUpdate()
	{
		base.StateUpdate();
		Crouching();

		if (uncrouch)
			player.stateMachine.ChangeState(player.moveState);
	}

	private void Crouching()
	{
		float newAcceleration = player.isGrounded ? player.acceleration : player.acceleration * 0.5f;

		if (Input.GetKey(KeyCode.LeftArrow) || Input.GetKey(KeyCode.A))
		{
			if (player.rb2D.velocity.x > 0)
				player.inputs.x = 0; //immediately stops and turns player

			player.inputs.x = Mathf.MoveTowards(player.inputs.x, -1, newAcceleration * Time.deltaTime);
		}
		else if (Input.GetKey(KeyCode.RightArrow) || Input.GetKey(KeyCode.D))
		{
			if (player.rb2D.velocity.x < 0)
				player.inputs.x = 0; //immediate stops and turns player

			player.inputs.x = Mathf.MoveTowards(player.inputs.x, 1, newAcceleration * Time.deltaTime);
		}
		else
		{
			player.inputs.x = Mathf.MoveTowards(player.inputs.x, 0, newAcceleration * 2 * Time.deltaTime);
		}

		Vector2 velocity = new Vector2(player.inputs.x * player.moveSpeed * player.crouchMoveSpeed, player.rb2D.velocity.y);
		player.rb2D.velocity = Vector2.MoveTowards(player.rb2D.velocity, velocity, player.moveLerpSpeed * Time.deltaTime);
	}

	private void Crouch()
	{
		player.playerCollider.direction = CapsuleDirection2D.Horizontal;
		player.playerCollider.size = player.crouchSize;
		player.playerCollider.offset = new Vector2(player.playerOffset.x, player.crouchOffset);
		player.anim.SetBool("Crouch", true);
	}

	private void Uncrouch()
	{
		player.playerCollider.direction = CapsuleDirection2D.Vertical;
		player.playerCollider.size = player.playerSize;
		player.playerCollider.offset = player.playerOffset;
		player.anim.SetBool("Crouch", false);
	}
}
