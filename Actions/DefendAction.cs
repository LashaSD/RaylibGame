using System.Numerics;
using Raylib_cs;

public class DefendAction : IAction
{
    public override Func<bool>? TerminationCondition { get; protected set; }

    public override float? Duration { get; protected set; } = 100f;
    public override float? DebounceTime { get; protected set; } = 5f;

    public override State? PostExecutionState { get; protected set; } = State.Defend;

    public override void Execute(Entity entity)
    {
        base.Execute(entity);
        MovementComponent? movementComponent = entity.GetComponent<MovementComponent>();
        this.TerminationCondition = () =>
        {
            if (movementComponent is null)
                return true;

            return movementComponent.MoveDirection != Vector2.Zero;
        };
    }
}
