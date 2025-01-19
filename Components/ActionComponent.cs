using Raylib_cs;
using System.Numerics;
using FarseerPhysics.Collision;

public abstract class IAction
{
    public virtual State? PreExecutionState { get; protected set; }
    public virtual State? PostExecutionState { get; protected set; }
    public virtual float? Duration { get; protected set; }
    public virtual float? DebounceTime { get; protected set; } = 0.25f;
    public virtual Func<bool>? TerminationCondition { get; protected set; }

    public virtual void Execute(Entity entity)
    { }
}

public class JumpAction : IAction
{
    public override State? PostExecutionState { get; protected set; } = State.Jump;
    public override float? Duration { get; protected set; } = 2.5f;
    public override float? DebounceTime { get; protected set; }

    private float JumpLength = 1f;

    public override void Execute(Entity entity)
    {
        DynamicBodyComponent? dynamicBody = entity.GetComponent<DynamicBodyComponent>();

        if (dynamicBody is null || dynamicBody.PhysicsBody is null)
            return;

        if (!dynamicBody.IsGrounded)
            return;

        Vector2 impulse = new Vector2(0, -3.0f);
        if (Math.Abs(dynamicBody.PhysicsBody.LinearVelocity.X) > 0.005f)
            impulse += new Vector2(Math.Sign(dynamicBody.PhysicsBody.LinearVelocity.X) * this.JumpLength, 0);

        Vector2 vel = PhysicsSystem.GetVelocity(dynamicBody.PhysicsBody);
        Vector2 v0 = Vector2.Divide(impulse, dynamicBody.PhysicsBody.Mass);
        this.DebounceTime = Math.Max(Math.Abs((2 * v0.Y) / PhysicsSystem.PhysicsWorld.Gravity.Y), 0.1f);
        this.Duration = this.DebounceTime;

        dynamicBody.PhysicsBody.ApplyLinearImpulse(PhysicsSystem.AsSimUnits(impulse));
    }
}

public class AttackAction1 : IAction
{
    public override State? PostExecutionState { get; protected set; } = State.Attack1;
    public override float? Duration { get; protected set; } = 0.55f;
    public override float? DebounceTime { get; protected set; } = 0.55f;
    public float Damage { get; protected set; } = 20;

    public override void Execute(Entity entity)
    {
        DynamicBodyComponent? dynamicBody = entity.GetComponent<DynamicBodyComponent>();
        TransformComponent? transform = entity.GetComponent<TransformComponent>();

        if (dynamicBody is null || dynamicBody.PhysicsBody is null || transform is null)
            return;
        
        Vector2 attackPosition = transform.Position;
        Vector2 attackRange = new(200, 200);
        AABB attackBox = new(
                PhysicsSystem.ToSimUnits(attackPosition - attackRange / 2),
                PhysicsSystem.ToSimUnits(attackPosition + attackRange / 2)
            );

        List<Entity> hitEntities = new List<Entity>();

        PhysicsSystem.PhysicsWorld.QueryAABB(fixture =>
            {
                if (fixture.Body.UserData is not Entity hitEntity)
                    return true;

                if (hitEntity.Id == entity.Id)
                    return true;

                hitEntities.Add(hitEntity);

                return true;
            }, 
            ref attackBox
        );

        // Apply damage to all hit entities
        foreach (Entity hitEntity in hitEntities)
        {
            var healthComponent = hitEntity.GetComponent<HealthComponent>();
            if (healthComponent != null)
            {
                Raylib.TraceLog(TraceLogLevel.Info, $"{entity.Id} {hitEntity.Id}");
                healthComponent.TakeDamage(this.Damage);
            }
        }

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

    public override void Destroy()
    {
        ActionSystem.Remove(this);
    }

    public override void Update(float deltaTime)
    {
        if (this.CurrentAction is null || this.StateComp is null)
            return;

        float duration = this.CurrentAction.Duration ?? 0.0f;
        float timeElapsed = (float) Raylib.GetTime() - this.ActionTimer;

        if (timeElapsed > duration)
        {
            this.ResetAction();
            return;
        }

        if (this.CurrentAction.TerminationCondition?.Invoke() ?? false)
            this.ResetAction();
    }

    private void ResetAction() 
    {
        if (this.CurrentAction is null)
            return;

        this.LastAction = this.CurrentAction;
        this.LastActionTimer = this.ActionTimer;

        this.CurrentAction = null;
        this.ActionTimer = 0.0f;

        this.StateComp?.SetState(State.Idle);
    }
}
