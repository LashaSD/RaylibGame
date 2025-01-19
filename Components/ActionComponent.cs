using Raylib_cs;

public abstract class IAction
{
    public virtual Func<bool>? TerminationCondition { get; protected set; }

    public virtual float? Delay { get; protected set; }
    public virtual float? Duration { get; protected set; }
    public virtual float? DebounceTime { get; protected set; } = 0.25f;

    public virtual State? PreExecutionState { get; protected set; }
    public virtual State? PostExecutionState { get; protected set; }

    public virtual bool Executed { get; set; } = false;

    public virtual void Execute(Entity entity)
    {
        this.Executed = true;
    }
}

public class ActionComponent : Component
{
    private StateComponent? StateComp;

    private IAction? CurrentAction;
    public Dictionary<string, float> ActionTimer = new();

    public void Execute(IAction action)
    {
        if (this.ParentEntity is null)
            return;

        string? newActionName = action.ToString();
        if (newActionName is null)
            throw new Exception($"Couldn't Get the Name of an Action {this.CurrentAction}");

        if (action.DebounceTime.HasValue)
        {
            this.ActionTimer.TryGetValue(newActionName, out float timer);
            if ((float) Raylib.GetTime() - timer < action.DebounceTime)
                return;
        }

        this.CurrentAction = action;
        if (!this.ActionTimer.ContainsKey(newActionName))
            this.ActionTimer.Add(newActionName, (float) Raylib.GetTime());
        else
            this.ActionTimer[newActionName] = (float) Raylib.GetTime();

        if (action.Delay is null)
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

    public override void Destroy()
    {
        ActionSystem.Remove(this);
    }

    public override void Update(float deltaTime)
    {
        if (this.CurrentAction is null || this.StateComp is null || this.ParentEntity is null)
            return;

        string? actionName = this.CurrentAction.ToString();
        if (actionName is null)
            throw new Exception($"Couldn't Get the Name of an Action {this.CurrentAction}");

        float duration = this.CurrentAction.Duration ?? 0.0f;
        float timeElapsed = (float) Raylib.GetTime() - this.ActionTimer[actionName];

        if (this.CurrentAction.Delay is not null)
        {
            if (timeElapsed > this.CurrentAction.Delay && !this.CurrentAction.Executed)
                this.CurrentAction.Execute(this.ParentEntity);
        }

        if (timeElapsed > duration)
        {
            this.ResetAction();
            return;
        }

        if (this.CurrentAction.TerminationCondition?.Invoke() ?? false)
            this.ResetAction();
    }

    public void ResetAction() 
    {
        if (this.CurrentAction is null)
            return;

        string? actionName = this.CurrentAction.ToString();
        if (actionName is null)
            throw new Exception($"Couldn't Get the Name of an Action {this.CurrentAction}");

        this.ActionTimer[actionName] = 0.0f;
        this.CurrentAction = null;

        this.StateComp?.SetState(State.Idle);
    }
}
