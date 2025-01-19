using System.Numerics;

public class TransformComponent : Component
{
    public Vector2 Position { get; private set; }
    public Vector2 Scale { get; private set; } = new Vector2(1, 1);
    public float Rotation { get; set; } =  0.0f;

    public Vector2 FaceDirection { get; set; } = new Vector2(1, 0);

    public void SetPosition(Vector2 pos)
    {
        this.Position = pos;
    }

    public void SetScale(Vector2 scale)
    {
        this.Scale = scale;
    }

    public void SetTransform(Vector2 pos, Vector2 scale)
    {
        this.Position = pos;
        this.Scale = scale;
    }

    public void SetRotation(float rotation)
    {
        this.Rotation = rotation;
    }

    public TransformComponent()
    {
        TransformSystem.Register(this);
    }

    public TransformComponent(Vector2 pos)
        : this()
    {
        this.SetPosition(pos);
    }

    public TransformComponent(Vector2 pos, Vector2 scale)
        : this()
    {
        this.SetTransform(pos, scale);
    }

    public override void Destroy()
    {
        TransformSystem.Remove(this);
    }
}
