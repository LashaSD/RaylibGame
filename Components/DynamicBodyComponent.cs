using System.Numerics;
using FarseerPhysics.Dynamics;

class DynamicBodyComponent : Component
{
    public TransformComponent? Transform;
    public RenderComponent? RenderComponent;

    public Vector2 BodySize;
    public Body? PhysicsBody { get; set; }

    private float Density { get; set; }
    public int Collisions { get; private set; } = 0;
    public bool IsGrounded { get => Collisions > 0;  }


    public Fixture? GroundSensor;

    public override void Init()
    {
        this.RenderComponent = this.ParentEntity?.GetComponent<RenderComponent>();
        this.Transform = this.ParentEntity?.GetComponent<TransformComponent>();
        if (this.Transform is null)
            return;

        var bodySize = PhysicsSystem.ToSimUnits(this.BodySize);
        this.PhysicsBody = PhysicsSystem.CreateDynamicBody(PhysicsSystem.ToSimUnits(this.Transform.Position), bodySize.X, bodySize.Y, this.Density);

        this.GroundSensor = PhysicsSystem.CreateGroundSensor(this.PhysicsBody, bodySize.X, bodySize.Y);
        this.GroundSensor.OnCollision += this.OnGroundCollision;
        this.GroundSensor.OnSeparation += this.OnGroundSeparation;
    }

    public override void Destroy()
    {
        DynamicBodySystem.Remove(this);
    }

    public override void Update(float deltaTime)
    {
        if (this.PhysicsBody is not null && this.Transform is not null)
        {
            this.Transform.SetPosition(PhysicsSystem.GetPosition(this.PhysicsBody));
            this.Transform.SetRotation((float)(180 / Math.PI) * this.PhysicsBody.Rotation);

            if (this.RenderComponent is not null)
            {
                if (this.PhysicsBody.LinearVelocity.X < -0.005f)
                    this.RenderComponent.Mirror = true;

                else if (this.PhysicsBody.LinearVelocity.X > 0.005f)
                    this.RenderComponent.Mirror = false;
            }
        }
    }

    private bool OnGroundCollision(Fixture fixtureA, Fixture fixtureB, FarseerPhysics.Dynamics.Contacts.Contact contact)
    {
        this.Collisions++;
        return true;
    }

    private void OnGroundSeparation(Fixture fixtureA, Fixture fixtureB)
    {
        this.Collisions--;
    }

    public DynamicBodyComponent(Vector2 size, float density)
    {
        this.Density = FarseerPhysics.ConvertUnits.ToSimUnits(density);
        this.BodySize = size;
        DynamicBodySystem.Register(this);
    }
}
