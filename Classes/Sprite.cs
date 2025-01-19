using Raylib_cs;
using System.Numerics;

public class Sprite
{
    public Texture2D Texture { get; }
    public Rectangle SourceRect { get; private set; }

    public Vector2 Origin { get; } = new Vector2(0.5f, 0.5f);
    public Vector2 Size { get => new Vector2(this.SourceRect.Width, this.SourceRect.Height); }
    public Vector2 SizePx { get => new Vector2(this.SourceRect.Width * this.Scale.X, this.SourceRect.Height * this.Scale.Y); }
    public Vector2 Scale { get; private set; } = new Vector2(1.0f, 1.0f);

    public Sprite(Texture2D texture, Rectangle sourceRect)
    {
        this.Texture = texture;
        this.SourceRect = sourceRect;
    }

    // Sets the Scale to achieve the given Size when multipled
    public void SetScaleForSize(Vector2 size)
    {
        this.Scale = size / this.Size;
    }

    public void SetScale(Vector2 size)
    {
        this.Scale = size;
    }
}
