using Raylib_cs;
using Microsoft.Xna.Framework;

public class RenderComponent : Component
{
    public Texture2D Texture { get; set; }

    public Rectangle SourceRect { get; set; }
    public System.Numerics.Vector2 Origin { get; set; } = new System.Numerics.Vector2(0.0f, 0.0f);

    public Shader? Shader { get; set; }

    public float Rotation { get; set; } = 0f;
    public float ZIndex { get; set; } = 0f;

    public bool Visible { get; set; } = true;

    public RenderComponent()
    {
        RenderSystem.Register(this);
    }

    public void SetSourceRect(Rectangle rect)
    {
        this.SourceRect = rect;
    }

    public void SetTexture(Texture2D texture)
    {
        this.Texture = texture;
    }

    public override void Update(float deltaTime)
    {
        if (this.ParentEntity is null)
            return;

        DynamicBodyComponent? dynamicBody = this.ParentEntity.GetComponent<DynamicBodyComponent>();
        StaticBodyComponent? staticBody = this.ParentEntity.GetComponent<StaticBodyComponent>();

        Vector2 Pos = new();
        Vector2 Size = new();

        if (dynamicBody is null && staticBody is null)
            return;

        if (dynamicBody is not null)
        {
            Pos = dynamicBody.PhysicsBody.Position;
            Size = dynamicBody.Size;
        }

        if (staticBody is not null)
        {
            Pos = staticBody.PhysicsBody.Position;
            Size = staticBody.Size;
        }

        if (this.Shader.HasValue)
            Raylib.BeginShaderMode(this.Shader.Value);

        Rectangle posRect = new Rectangle(
            Pos.X,
            Pos.Y,
            Size.X,
            Size.Y
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
