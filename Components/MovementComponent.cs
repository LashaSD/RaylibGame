using System.Numerics;
using Raylib_cs;

public class MovementComponent : Component
{
    private DynamicBodyComponent? DynamicBodyComponent { get; set; }
    private StateComponent? StateComponent { get; set; }

    public Vector2 MoveDirection { get; private set; } = new(0, 0);
    public float Speed { get; private set; } = 2.5f;
    public bool IsMoving { get; private set; } = false;

    private float AirStrafingCoefficient = 0.75f;

    public MovementComponent()
    { }

    public override void Init()
    {
        this.DynamicBodyComponent = this.ParentEntity?.GetComponent<DynamicBodyComponent>();
        this.StateComponent = this.ParentEntity?.GetComponent<StateComponent>();
    }

    public void Update(float deltaTime, Vector2 direction)
    {
        if (this.ParentEntity is null)
            return;

        if (this.DynamicBodyComponent is null || this.DynamicBodyComponent.PhysicsBody is null)
            return;

        bool wasMoving = this.IsMoving;
        this.IsMoving = false;
        if (direction != Vector2.Zero)
        {
            this.MoveDirection = Vector2.Normalize(direction);
            this.IsMoving = true;
        }

        if (this.IsMoving)
        {
            Vector2 desiredVelocity = this.MoveDirection * this.Speed;
            var vel = this.DynamicBodyComponent.PhysicsBody.LinearVelocity;
            if (this.DynamicBodyComponent.IsGrounded)
                this.DynamicBodyComponent.PhysicsBody.LinearVelocity = new(PhysicsSystem.AsSimUnits(desiredVelocity).X, vel.Y);
            else
                this.DynamicBodyComponent.PhysicsBody.LinearVelocity = new(PhysicsSystem.AsSimUnits(desiredVelocity).X * this.AirStrafingCoefficient, vel.Y);

            if (this.DynamicBodyComponent.IsGrounded && this.StateComponent?.CurrentState != State.Run && this.StateComponent?.CurrentState != State.Jump)
                this.StateComponent?.SetState(State.Run);
        } 
        else 
        {
            if (!wasMoving)
                return;

            if (this.StateComponent is not null && this.StateComponent.CurrentState == State.Run)
                // Stopped Moving so change the State to Idle
                this.StateComponent?.SetState(State.Idle);
        }
    }
}
