using Raylib_cs;
using System.Numerics;

public class RenderComponent : Component
{
    public Sprite? Sprite { get; private set; }
    public TileMap? TileMap { get; private set; }
    public Shader? Shader { get; set; }

    private TransformComponent? Transform;
    private DynamicBodyComponent? DynamicBody;
    private HealthComponent? HealthComponent;

    public bool Mirror { get; set; } = false;

    public RenderComponent()
    {
        RenderSystem.Register(this);
    }

    private void UpdateSprite()
    {
        // update the sprite for when the animation system changes it 
        if (this.Transform is null || this.Sprite is null)
            return;

        this.Sprite.SetScale(this.Transform.Scale);
    }

    public void SetSprite(Sprite sprite)
    {
        this.Sprite = sprite;
        this.UpdateSprite();
    }

    public void SetTileMap(TileMap tileMap)
    {
        this.TileMap = tileMap;
        // not updating the sprites because tileMap will only be used for static bodies
    }

    public override void Init()
    {
        this.Transform = this.ParentEntity?.GetComponent<TransformComponent>();
        this.DynamicBody = this.ParentEntity?.GetComponent<DynamicBodyComponent>();
        this.HealthComponent = this.ParentEntity?.GetComponent<HealthComponent>();
        this.UpdateSprite();
    }

    public override void Destroy()
    {
        RenderSystem.Remove(this);
    }

    public override void Update(float deltaTime)
    {
        if (this.Transform is null || (this.Sprite is null && this.TileMap is null))
            return;

        if (this.Shader.HasValue)
            Raylib.BeginShaderMode(this.Shader.Value);

        if (this.Sprite is not null)
            this.RenderSprite(this.Sprite);
        else if (this.TileMap is not null)
            this.RenderSpriteTileMap(this.TileMap);

        // DBG
        // if (this.DynamicBody is not null && this.DynamicBody.GroundSensor is not null)
        // {
        //     var pos = PhysicsSystem.ToDisplayUnits(this.DynamicBody.GroundSensor.Body.Position);
        //     var size = this.DynamicBody.BodySize;
        //     Raylib.DrawRectangleV(pos + new Vector2( - (size.X * 0.4f) / 2, size.Y / 2), new(size.X * 0.4f, 5f), Color.Blue);
        //
        //     var pos1 = pos+ new Vector2(-size.X / 2, - size.Y / 2);
        //     Raylib.DrawText($"{this.DynamicBody?.Collisions} {this.DynamicBody?.IsGrounded}", (int) pos1.X, (int) pos1.Y, 16, Color.Gold);
        // }
        
        if (this.HealthComponent is not null && this.DynamicBody is not null)
        {
            var pos = this.Transform.Position;
            var size = this.DynamicBody.BodySize;
            var pos1 = pos + new Vector2(-size.X / 2, -size.Y / 2);

            Color color = Color.Red;

            if (this.HealthComponent.Health > 40) {
                color = Color.Yellow;
            }

            if (this.HealthComponent.Health > 70) {
                color = Color.Green;
            }

            Raylib.DrawText($"{this.HealthComponent.Health}", (int) pos1.X, (int) pos1.Y, 16, color);
        }

        if (this.Shader.HasValue)
        {
            Raylib.EndShaderMode();
        }
    }


    private void RenderSprite(Sprite sprite)
    {
        if (this.Transform is null)
            return;

        Rectangle destRect = new Rectangle(
            this.Transform.Position.X,
            this.Transform.Position.Y,
            sprite.Size.X * sprite.Scale.X,
            sprite.Size.Y * sprite.Scale.Y
        );
        
        Rectangle srcRect = sprite.SourceRect;
        srcRect.Width *= this.Transform.FaceDirection.X;
        
        Raylib.DrawTexturePro(
            sprite.Texture,
            srcRect,
            destRect,
            new Vector2(destRect.Width * sprite.Origin.X, destRect.Height * sprite.Origin.Y),
            this.Transform.Rotation,
            Color.White
        );
    }

    private void RenderTile(Sprite sprite, Vector2 offset, Vector2 TileMapSize)
    {
        if (this.Transform is null)
            return;

        Rectangle destRect = new Rectangle(
            this.Transform.Position.X - (TileMapSize.X / 2) + (sprite.SizePx.X / 2) + offset.X,
            this.Transform.Position.Y - (TileMapSize.Y / 2) + (sprite.SizePx.Y / 2) + offset.Y,
            sprite.SizePx.X,
            sprite.SizePx.Y
        );
        
        Rectangle srcRect = sprite.SourceRect;
        srcRect.Width *= this.Transform.FaceDirection.X;
        
        Raylib.DrawTexturePro(
            sprite.Texture,
            srcRect,
            destRect,
            new Vector2(destRect.Width * sprite.Origin.X, destRect.Height * sprite.Origin.Y),
            this.Transform.Rotation,
            Color.White
        );
    }

    private void RenderSpriteTileMap(TileMap tileMap)
    {
        Vector2 offset = new(0, 0);
        foreach (List<Sprite> spriteRow in tileMap.Sprites)
        {
            foreach (Sprite sprite in spriteRow)
            {
                this.RenderTile(sprite, offset, tileMap.Size);
                offset += new Vector2(sprite.SizePx.X, 0);
            }

            if (spriteRow.Count > 0)
            {
                offset = new Vector2(0, offset.Y + spriteRow.First().SizePx.Y);
            }
        }
    }
}
