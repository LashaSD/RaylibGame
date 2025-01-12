using Raylib_cs;

public class KeyFrames
{
    public Texture2D Texture { get; set; }
    public Rectangle SpriteRect { get; set; }

    public KeyFrames(Texture2D texture, Rectangle spriteRect)
    {
        this.Texture = texture;
        this.SpriteRect = spriteRect;
    }

    public void Next()
    {
        this.SpriteRect = new Rectangle(this.SpriteRect.X + this.SpriteRect.Width, 0, this.SpriteRect.Width, this.SpriteRect.Height);
    }

    public void NextCyclic()
    {
        float offsetX = this.SpriteRect.X + this.SpriteRect.Width;
        bool withinWidth = (offsetX + this.SpriteRect.Width) <= this.Texture.Width;
        if (!withinWidth)
            offsetX = 0;
        
        this.SpriteRect = new Rectangle(offsetX, 0, this.SpriteRect.Width, this.SpriteRect.Height);
    }
}
