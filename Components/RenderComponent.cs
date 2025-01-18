using Raylib_cs;
using System.Numerics;

public class RenderComponent : Component
{
    public Sprite? Sprite { get; private set; }
    public TileMap? TileMap { get; private set; }
    public Shader? Shader { get; set; }

    public TransformComponent? Transform;

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
        this.UpdateSprite();
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
        if (this.Mirror)
            srcRect.Width *= -1;
        
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
        if (this.Mirror)
            srcRect.Width *= -1;
        
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
