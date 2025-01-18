using System.Numerics;
using FarseerPhysics.Dynamics;

class DynamicBodyComponent : Component
{
    public TransformComponent? Transform;
    public RenderComponent? RenderComponent;

    public Vector2 BodySize;
    public Body? PhysicsBody { get; set; }

    private float Density { get; set; }
    public bool IsGrounded { get; private set; }

    private Fixture? GroundSensor;

    public override void Init()
    {
        this.RenderComponent = this.ParentEntity?.GetComponent<RenderComponent>();
        this.Transform = this.ParentEntity?.GetComponent<TransformComponent>();
        if (this.Transform is null)
            return;

        var bodySize = PhysicsSystem.ToSimUnits(this.BodySize);
        this.PhysicsBody = PhysicsSystem.CreateDynamicBody(PhysicsSystem.ToSimUnits(this.Transform.Position), bodySize.X, bodySize.Y, this.Density);

        this.GroundSensor = PhysicsSystem.CreateGroundSensor(this.PhysicsBody, bodySize.X, 0.1f);
        this.GroundSensor.OnCollision += this.OnGroundCollision;
        this.GroundSensor.OnSeparation += this.OnGroundSeparation;
    }

    public override void Update(float deltaTime)
    {
        if (this.PhysicsBody is not null && this.Transform is not null)
        {
            this.Transform.SetPosition(PhysicsSystem.GetPosition(this.PhysicsBody));

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
        this.IsGrounded = true;
        return true;
    }

    private void OnGroundSeparation(Fixture fixtureA, Fixture fixtureB)
    {
        this.IsGrounded = false;
    }

    public DynamicBodyComponent(Vector2 size, float density)
    {
        this.Density = FarseerPhysics.ConvertUnits.ToSimUnits(density);
        this.BodySize = size;
        DynamicBodySystem.Register(this);
    }
}
