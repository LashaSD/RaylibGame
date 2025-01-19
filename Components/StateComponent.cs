using Raylib_cs;

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
    public Signal<State> StateChanged = new Signal<State>();
    public State CurrentState { get; private set; }
    public string Type { get; private set; }

    public void SetState(State newState)
    {
        this.CurrentState = newState;
        this.OnStateChanged(newState);
        this.StateChanged.Fire(newState);
    }

    public StateComponent(string Type)
    {
        this.Type = Type;
        StateSystem.Register(this);
    }

    public override void Init()
    {
        this.SetState(State.Idle);
        this.OnStateChanged(State.Idle);
    }

    public override void Destroy()
    {
        StateSystem.Remove(this);
    }

    private void OnStateChanged(State newState) 
    {
        if (this.ParentEntity is null)
            return;

        AnimationComponent? animComponent = this.ParentEntity.GetComponent<AnimationComponent>();

        if (animComponent is null)
            return;

        Animation? anim = Settings.MapStateToAnim(this.Type, newState);
        animComponent.LoadAnim(anim);
        anim.Play();
    }
}
