using Microsoft.Xna.Framework;
using FarseerPhysics.Dynamics;
using FarseerPhysics.Factories;

class DynamicBodyComponent : Component
{
    public Vector2 Pos { get; set; }
    public Vector2 Size { get; set; }
    public Body PhysicsBody { get; set; }

    public DynamicBodyComponent(Vector2 pos, Vector2 size, float density)
    {
        this.Pos = pos;
        this.Size = size;
        this.PhysicsBody = PhysicsSystem.CreateDynamicBody(Pos, Size.X, Size.Y, density);
    }
}
