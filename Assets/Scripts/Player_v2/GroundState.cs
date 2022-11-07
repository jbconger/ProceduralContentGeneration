using UnityEngine;

public class GroundState : State
{
	private readonly Collider2D[] ground = new Collider2D[1];

    public GroundState(Player player) : base(player) { }

	public override void Enter() 
	{ 
		base.Enter();
	}

	public override void Exit()
	{
		base.Exit();
	}

	public override void HandleInput()
	{
		base.HandleInput();
	}

	public override void StateUpdate()
	{
		base.StateUpdate();
		Grounding();
		Falling();
	}

	private void Grounding()
	{
		bool grounded = Physics2D.OverlapCircleNonAlloc(player.transform.position + new Vector3(0, player.groundOffset), player.groundRadius, ground, player.groundMask) > 0;

		if (!player.isGrounded && grounded)
		{
			player.isGrounded = true;
			player.anim.SetBool("Grounded", true);
			player.anim.SetBool("Fall", false);
			player.anim.SetBool("Jump", false);
			player.playerAudio.PlayGroundSound();
		}
		else if (player.isGrounded && !grounded)
		{
			player.isGrounded = false;
			player.anim.SetBool("Grounded", false);
		}
	}

	private void Falling()
	{
		if (player.rb2D.velocity.y < player.jumpVelocityFalloff || player.rb2D.velocity.y > 0 && (!Input.GetKey(KeyCode.Space) && !Input.GetKey(KeyCode.Z)))
		{
			player.rb2D.velocity += player.fallMultiplier * Physics.gravity.y * Vector2.up * Time.deltaTime;
		}

		if (player.rb2D.velocity.y < -1.5)
		{
			player.anim.SetBool("Jump", false);
			player.anim.SetBool("Fall", true);
		}
	}
}
