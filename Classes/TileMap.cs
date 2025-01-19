
using Raylib_cs;
using System.Numerics;

public class TileMap
{
    public Texture2D Texture { get; }
    public List<List<Sprite>> Sprites { get; private set; } = new();
    
    public Vector2 Size { get; private set; }

    public TileMap(Vector2 size)
    {
        this.Size = size;
    }

    public void AddSpriteRow(List<Sprite> sprites)
    {
        this.Sprites.Add(sprites);
    }
}
