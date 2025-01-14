using Raylib_cs;
using System.Numerics;

public class RenderComponent : Component
{
    public Sprite? Sprite { get; private set; }
    public Shader? Shader { get; set; }

    public TransformComponent? Transform;

    public RenderComponent()
    {
        RenderSystem.Register(this);
    }

    private void UpdateSprite()
    {
        if (this.Transform is null || this.Sprite is null)
            return;

        this.Sprite.SetScale(this.Transform.Scale);
    }

    public void SetSprite(Sprite sprite)
    {
        this.Sprite = sprite;
        this.UpdateSprite();
    }

    public override void Init()
    {
        this.Transform = this.ParentEntity?.GetComponent<TransformComponent>();
        this.UpdateSprite();
    }

    public override void Update(float deltaTime)
    {
        if (this.Transform is null || this.Sprite is null)
            return;

        if (this.Shader.HasValue)
            Raylib.BeginShaderMode(this.Shader.Value);

        Rectangle destRect = new Rectangle(
            this.Transform.Position.X,
            this.Transform.Position.Y,
            this.Sprite.Size.X * this.Sprite.Scale.X,
            this.Sprite.Size.Y * this.Sprite.Scale.Y
        );
        
        Raylib.DrawTexturePro(
            this.Sprite.Texture,
            this.Sprite.SourceRect,
            destRect,
            new Vector2(destRect.Width * this.Sprite.Origin.X, destRect.Height * this.Sprite.Origin.Y),
            this.Transform.Rotation,
            Color.White
        );

        if (this.Shader.HasValue)
        {
            Raylib.EndShaderMode();
        }
    }
}
