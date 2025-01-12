using Microsoft.Xna.Framework;

public class TransformComponent : Component
{
    public Vector2 Pos = new();
    public Vector2 Scale = new Vector2(1.0f, 1.0f);

    public void SetPos(Vector2 position)
    {
        this.Pos = position;
    }

    public void SetScale(Vector2 scale)
    {
        this.Scale = scale;
    }

    public TransformComponent()
    {
        TransformSystem.Register(this);
    }
}
