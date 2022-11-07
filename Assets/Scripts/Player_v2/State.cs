public class State
{
    protected Player player;

    public State(Player player)
	{
		this.player = player;
	}

	public virtual void Enter() { }
	public virtual void Exit() { }
	public virtual void HandleInput() { }
	public virtual void StateUpdate() { }
}
