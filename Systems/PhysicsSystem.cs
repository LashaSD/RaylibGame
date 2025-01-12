using FarseerPhysics.Dynamics;
using FarseerPhysics.Factories;
using Microsoft.Xna.Framework;

class PhysicsSystem
{
    public static World PhysicsWorld { get; private set; } = new World(new Vector2(0, 9.8f * 15));

    public static void SetGravity(Vector2 gravity)
    {
        PhysicsWorld.Gravity = gravity;
    }

    public static void Update(float deltaTime)
    {
        PhysicsWorld.Step(deltaTime);
    }

    public static Body CreateDynamicBody(Vector2 position, float width, float height, float density)
    {
        Body body = BodyFactory.CreateRectangle(PhysicsWorld, width, height, density);
        body.BodyType = BodyType.Dynamic;
        body.Position = position;
        return body;
    }

    public static Body CreateStaticBody(Vector2 position, float width, float height, float density)
    {
        var body = BodyFactory.CreateRectangle(PhysicsWorld, width, height, density);
        body.Position = position;
        body.BodyType = BodyType.Static;
        return body;
    }
}

