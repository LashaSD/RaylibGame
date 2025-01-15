using FarseerPhysics.Dynamics;
using FarseerPhysics.Factories;
using Microsoft.Xna.Framework;

class PhysicsSystem
{
    public static World PhysicsWorld { get; private set; } = new World(new Vector2(0, 9.8f));

    public static void Init()
    {
        FarseerPhysics.ConvertUnits.SetDisplayUnitToSimUnitRatio(100);
    }

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
        Body body = BodyFactory.CreateRectangle(PhysicsWorld, width, height, density);
        body.BodyType = BodyType.Static;
        body.Position = position;
        return body;
    }

    public static Fixture CreateGroundSensor(Body body, float width, float height)
    {
        Fixture sensor = FixtureFactory.AttachRectangle(
            width,
            height,
            0,
            offset: new Vector2(0, body.FixtureList[0].Shape.Radius + height / 2 + 0.5f),
            body
        );

        sensor.IsSensor = true;
        return sensor;
    }

    public static System.Numerics.Vector2 GetPosition(Body body)
    {
        Vector2 pos = FarseerPhysics.ConvertUnits.ToDisplayUnits(body.Position);
        return new System.Numerics.Vector2(pos.X, pos.Y);
    }

    public static System.Numerics.Vector2 GetVelocity(Body body)
    {
        Vector2 pos = FarseerPhysics.ConvertUnits.ToDisplayUnits(body.LinearVelocity);
        return new System.Numerics.Vector2(pos.X, pos.Y);
    }

    public static Vector2 ToSimUnits(System.Numerics.Vector2 vector)
    {
        Vector2 simVector = FarseerPhysics.ConvertUnits.ToSimUnits(new Vector2(vector.X, vector.Y));
        return simVector;
    }

    public static float ToSimUnits(float num)
    {
        return FarseerPhysics.ConvertUnits.ToSimUnits(num);
    }

    public static Vector2 AsSimUnits(System.Numerics.Vector2 vector)
    {
        Vector2 simVector = new Vector2(vector.X, vector.Y);
        return simVector;
    }
}

