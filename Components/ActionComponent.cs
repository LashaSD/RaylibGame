using Raylib_cs;
using System.Numerics;

public abstract class IAction
{
    public virtual State? PostExecutionState { get; protected set; }
    public virtual float? Duration { get; protected set; }
    public virtual void Execute(Entity entity)
    { }
}

public class JumpAction : IAction
{
    public override State? PostExecutionState { get; protected set; } = State.Jump;
    public override float? Duration { get; protected set; } = 2.5f;

    public override void Execute(Entity entity)
    {
        TransformComponent? transform = entity.GetComponent<TransformComponent>();

        if (transform is null)
            return;

        Vector2 pos = transform.Position;
        transform.SetPos(new Vector2(pos.X, pos.Y + 15.0f));
    }
}

public class ActionComponent : Component
{
    private StateComponent? StateComp;
    private IAction? CurrentAction;
    private float ActionTimer;

    public void Execute(IAction action)
    {
        if (this.ParentEntity is not null)
        {
            this.CurrentAction = action;
            this.ActionTimer = (float) Raylib.GetTime();

            action.Execute(this.ParentEntity);

            if (action.PostExecutionState is not null)
                this.StateComp?.SetState((State) action.PostExecutionState);
        }
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
            this.StateComp?.SetState(State.Run_Attack);
        }
    }
}
