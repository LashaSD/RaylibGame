using System.Numerics;
using Raylib_cs;

public class RenderComponent : Component
{
    public Texture2D Texture { get; set; }

    public Rectangle SourceRect { get; set; }
    public Vector2 Origin { get; set; }

    public Shader? Shader { get; set; }

    public float Rotation { get; set; } = 0f;
    public float ZIndex { get; set; } = 0f;

    public bool Visible { get; set; } = true;

    public override void Init()
    {

    }

    public override void Update(float deltaTime)
    {
        
    }
}
