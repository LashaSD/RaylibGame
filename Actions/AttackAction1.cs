using System.Numerics;

public class AttackAction1 : IAction
{
    public override State? PostExecutionState { get; protected set; } = State.Attack1;
    public override float? Delay { get; protected set; } = 0.75f;
    public override float? Duration { get; protected set; } = 0.75f;
    public override float? DebounceTime { get; protected set; } = 0.75f;

    public float Damage { get; protected set; } = 50;
    public float Pushback { get; protected set; } = 100;

    public Vector2 AttackRange { get; protected set; } = new(50, 50);

    public override void Execute(Entity entity)
    {
        base.Execute(entity);
        AttackHelper.GeneralAttack(entity, this.AttackRange, this.Damage, this.Pushback);
    }
}
