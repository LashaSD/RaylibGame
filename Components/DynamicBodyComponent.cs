using System.Numerics;
using FarseerPhysics.Dynamics;
using FarseerPhysics.Factories;

class DynamicBodyComponent : Component
{
    public TransformComponent? Transform;
    public Vector2 BodySize;
    public Body? PhysicsBody { get; set; }

    private float Density { get; set; }

    public override void Init()
    {
        this.Transform = this.ParentEntity?.GetComponent<TransformComponent>();
        if (this.Transform is not null)
            this.PhysicsBody = PhysicsSystem.CreateDynamicBody(PhysicsSystem.NumericToMicrosoft(this.Transform.Position), this.BodySize.X, this.BodySize.Y, this.Density);
    }

    public override void Update(float deltaTime)
    {
        if (this.PhysicsBody is not null)
            this.Transform?.SetPosition(PhysicsSystem.MicrosoftToNumeric(this.PhysicsBody.Position));
    }

    public DynamicBodyComponent(Vector2 size, float density)
    {
        this.Density = density;
        this.BodySize = size;
        DynamicBodySystem.Register(this);
    }
}
