using System.Numerics;

public class AttackAction2 : IAction
{
    public override State? PostExecutionState { get; protected set; } = State.Attack2;
    public override float? Delay { get; protected set; } = 0.15f;
    public override float? Duration { get; protected set; } = 0.35f;
    public override float? DebounceTime { get; protected set; } = 0.35f;

    public float Damage { get; protected set; } = 20;
    public float Pushback { get; protected set; } = 50;

    public Vector2 AttackRange { get; protected set; } = new(50, 50);

    public override void Execute(Entity entity)
    {
        base.Execute(entity);
        AttackHelper.GeneralAttack(entity, this.AttackRange, this.Damage, this.Pushback);
    }
}
