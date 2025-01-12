using System.Numerics;
using Raylib_cs;

public class RenderComponent : Component
{
    public Texture2D Texture { get; set; }

    public Rectangle SourceRect { get; set; }
    public Vector2 Origin { get; set; } = new Vector2(0.0f, 0.0f);

    public Shader? Shader { get; set; }

    public float Rotation { get; set; } = 0f;
    public float ZIndex { get; set; } = 0f;

    public bool Visible { get; set; } = true;

    public RenderComponent()
    {
        RenderSystem.Register(this);
    }

    public override void Update(float deltaTime)
    {
        if (this.ParentEntity is null)
            return;

        TransformComponent? transform = this.ParentEntity.GetComponent<TransformComponent>();
        if (transform is null)
            return;

        if (this.Shader.HasValue)
            Raylib.BeginShaderMode(this.Shader.Value);

        Rectangle posRect = new Rectangle(
            transform.Position.X,
            transform.Position.Y,
            this.SourceRect.Width * transform.Scale.X,
            this.SourceRect.Height * transform.Scale.Y
        );

        Raylib.DrawTexturePro(
            this.Texture,
            this.SourceRect,
            posRect,
            this.Origin,
            this.Rotation,
            Color.White
        );

        if (this.Shader.HasValue)
        {
            Raylib.EndShaderMode();
        }
    }
}
