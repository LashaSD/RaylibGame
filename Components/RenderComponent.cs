using Raylib_cs;
using Microsoft.Xna.Framework;

public class RenderComponent : Component
{
    public Sprite? Sprite { get; private set; }
    public Shader? Shader { get; set; }

    public TransformComponent? Transform;

    public RenderComponent()
    {
        RenderSystem.Register(this);
    }

    public void SetSprite(Sprite sprite)
    {
        this.Sprite = sprite;
    }

    public override void Init()
    {
        this.Transform = this.ParentEntity?.GetComponent<TransformComponent>();
    }

    public override void Update(float deltaTime)
    {
        if (this.Transform is null || this.Sprite is null)
            return;

        if (this.Shader.HasValue)
            Raylib.BeginShaderMode(this.Shader.Value);

        Rectangle posRect = new Rectangle(
            this.Transform.Position.X,
            this.Transform.Position.Y,
            this.Sprite.Size.X * this.Sprite.Scale.X,
            this.Sprite.Size.Y * this.Sprite.Scale.Y
        );
        
        Raylib.DrawTexturePro(
            this.Sprite.Texture,
            this.Sprite.SourceRect,
            posRect,
            this.Sprite.Origin,
            this.Transform.Rotation,
            Color.White
        );

        if (this.Shader.HasValue)
        {
            Raylib.EndShaderMode();
        }
    }
}
