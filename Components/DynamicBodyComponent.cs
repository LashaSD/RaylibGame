using System.Numerics;
using FarseerPhysics.Dynamics;

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
        {
            var bodySize = PhysicsSystem.ToSimUnits(this.BodySize);
            this.PhysicsBody = PhysicsSystem.CreateDynamicBody(PhysicsSystem.ToSimUnits(this.Transform.Position), bodySize.X, bodySize.Y, this.Density);
        }
    }

    public override void Update(float deltaTime)
    {
        if (this.PhysicsBody is not null)
        {
            this.Transform?.SetPosition(PhysicsSystem.GetPosition(this.PhysicsBody));
        }
    }

    public DynamicBodyComponent(Vector2 size, float density)
    {
        this.Density = density;
        this.BodySize = size;
        DynamicBodySystem.Register(this);
    }
}
