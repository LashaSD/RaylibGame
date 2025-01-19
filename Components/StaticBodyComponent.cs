using System.Numerics;
using FarseerPhysics.Dynamics;
using FarseerPhysics.Factories;

class StaticBodyComponent : Component
{
    public TransformComponent? Transform;
    public Vector2 BodySize;
    public Body? PhysicsBody { get; set; }

    private float Density { get; set; } = 1.0f;
    public float Friction { get; set; } = 25.0f;


    public StaticBodyComponent(Vector2 size)
    {
        this.BodySize = size;
    }

    public StaticBodyComponent(Vector2 size, float density)
        : this(size)
    {
        this.Density = density;
    }

    public override void Init()
    {
        this.Transform = this.ParentEntity?.GetComponent<TransformComponent>();
        if (this.Transform is not null)
        {
            var bodySize = PhysicsSystem.ToSimUnits(this.BodySize);
            this.PhysicsBody = PhysicsSystem.CreateStaticBody(PhysicsSystem.ToSimUnits(this.Transform.Position), bodySize.X, bodySize.Y, this.Density);
            this.PhysicsBody.Friction = FarseerPhysics.ConvertUnits.ToSimUnits(this.Friction);
        }

        if (this.PhysicsBody is not null)
        {
            this.Transform?.SetPosition(PhysicsSystem.GetPosition(this.PhysicsBody));
        }
    }
}
