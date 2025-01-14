using System.Numerics;
using Raylib_cs;

public class MovementComponent : Component
{
    private DynamicBodyComponent? DynamicBodyComponent { get; set; }
    private StateComponent? StateComponent { get; set; }

    public bool IsMoving { get; private set; } = false;
    public Vector2 MoveDirection { get; private set; } = new(0, 0);
    public float Speed { get; private set; } = 500.0f;

    public MovementComponent()
    {
        MovementSystem.Register(this);
    }

    public override void Init()
    {
        this.DynamicBodyComponent = this.ParentEntity?.GetComponent<DynamicBodyComponent>();
        this.StateComponent = this.ParentEntity?.GetComponent<StateComponent>();
    }

    public override void Update(float deltaTime)
    {
        if (this.ParentEntity is null)
            return;

        bool wasMoving = this.IsMoving;
        this.IsMoving = false;

        foreach (KeyboardKey key in Settings.MovementKeybinds.Keys)
        {
            if (Raylib.IsKeyDown(key))
            {
                this.MoveDirection = Settings.MovementKeybinds[key];
                this.IsMoving = true;
            }
        }

        if (this.IsMoving)
        {
            if (!wasMoving)
                // Started Moving so change the State to moving
                this.StateComponent?.SetState(State.Run);

            this.DynamicBodyComponent?.PhysicsBody?.ApplyForce(PhysicsSystem.ToSimUnits(this.MoveDirection * this.Speed));
        } else 
        {
            if (wasMoving)
                // Stopped Moving so change the State to Idle
                this.StateComponent?.SetState(State.Idle);
        }
    }
}
