public enum State
{
    Attack1,
    Attack2,
    Attack3,
    Dead,
    Defend,
    Hurt,
    Idle,
    Jump,
    Protect,
    Run,
    Run_Attack,
    Walk
}

public class StateComponent : Component
{
    public State CurrentState { get; set; } = State.Attack1;
    public Signal<State> StateChanged = new Signal<State>();

    public void SetState(State newState)
    {
        if (this.CurrentState != newState)
        {
            this.CurrentState = newState;
            this.StateChanged.Fire(newState);
        }
    }
}
