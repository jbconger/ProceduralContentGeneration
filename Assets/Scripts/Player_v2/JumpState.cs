using UnityEngine;

public class JumpState : MoveState
{
	private bool hasDoubleJumped;

    public JumpState(Player player) : base(player) { }

	public override void Enter()
	{
		base.Enter();
		hasDoubleJumped = false;
		Jump();
	}

	public override void Exit()
	{
		base.Exit();
		player.anim.SetBool("Jump", false);
	}

	public override void HandleInput()
	{
		//base.HandleInput();

		if ((Input.GetKeyDown(KeyCode.Space) || Input.GetKeyDown(KeyCode.Z)) && player.canDoubleJump)
		{
			if (!hasDoubleJumped)
			{
				hasDoubleJumped = true;
				Jump();

			}
		}
	}

	public override void StateUpdate()
	{
		base.StateUpdate();

		if (player.isGrounded && Mathf.Abs(player.rb2D.velocity.y) < 1.5)
			player.stateMachine.ChangeState(player.moveState);
	}

	private void Jump()
	{
		player.rb2D.velocity = new Vector2(player.rb2D.velocity.x, player.jumpForce);
		player.anim.SetBool("Fall", false);
		player.anim.SetBool("Jump", true);
		player.playerAudio.PlayJumpSound();
	}
}
