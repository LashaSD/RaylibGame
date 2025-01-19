using System.Numerics;
using System.Numerics;
using FarseerPhysics.Collision;

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
