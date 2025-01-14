using Raylib_cs;
using System.Numerics;

public abstract class IAction
{
    public virtual State? PreExecutionState { get; protected set; }
    public virtual State? PostExecutionState { get; protected set; }
    public virtual float? Duration { get; protected set; }
    public virtual float? DebounceTime { get; protected set; } = 0.25f;

    public virtual void Execute(Entity entity)
    { }
}

public class JumpAction : IAction
{
    public override State? PostExecutionState { get; protected set; } = State.Jump;
    public override float? Duration { get; protected set; } = 2.5f;
    public override float? DebounceTime { get; protected set; } = 1.25f;

    public override void Execute(Entity entity)
    {
        DynamicBodyComponent? dynamicBody = entity.GetComponent<DynamicBodyComponent>();

        if (dynamicBody is null)
            return;

        dynamicBody.PhysicsBody?.ApplyForce(PhysicsSystem.ToSimUnits(new Vector2(0, -20000f)));
    }
}

public class ActionComponent : Component
{
    private StateComponent? StateComp;

    private IAction? CurrentAction;
    private float ActionTimer = -100.0f;

    private IAction? LastAction;
    private float? LastActionTimer;

    public void Execute(IAction action)
    {
        if (this.ParentEntity is null)
            return;

        if (this.CurrentAction is not null && action == this.CurrentAction)
        {
            if (action.DebounceTime.HasValue)
            {
                if ((float) Raylib.GetTime() - this.ActionTimer < action.DebounceTime)
                    return;
            }
        }

        this.CurrentAction = action;
        this.ActionTimer = (float) Raylib.GetTime();

        action.Execute(this.ParentEntity);

        if (action.PostExecutionState is not null)
            this.StateComp?.SetState((State) action.PostExecutionState);
    }

    public ActionComponent()
    {
        ActionSystem.Register(this);
    }

    public override void Init()
    {
        this.StateComp = this.ParentEntity?.GetComponent<StateComponent>();
    }

    public override void Update(float deltaTime)
    {
        if (this.CurrentAction is null || this.StateComp is null)
            return;

        float duration = this.CurrentAction.Duration ?? 0.0f;
        float timeElapsed = (float) Raylib.GetTime() - this.ActionTimer;
        if (timeElapsed > duration)
        {
            this.LastAction = this.CurrentAction;
            this.LastActionTimer = this.ActionTimer;

            this.CurrentAction = null;
            this.ActionTimer = 0.0f;

            this.StateComp?.SetState(State.Idle);
        }
    }
}
