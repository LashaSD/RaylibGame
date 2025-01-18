using Raylib_cs;

public static class TextureManager
{
    private static Dictionary<string, Texture2D> Textures = new();
    
    public static void LoadTextures()
    {
        string baseDirectory = Environment.CurrentDirectory;
        string folderName = "Assets";
        string directoryPath = Path.Join(baseDirectory, folderName);

        string[] files = Directory.GetFiles(directoryPath);
        string[] directories = Directory.GetDirectories(directoryPath);

        foreach (string fileDir in files)
        {
            Textures.Add(Path.GetFileName(fileDir), Raylib.LoadTexture(fileDir));
        }

        foreach (string dir in directories)
        {
            foreach (string fileDir in Directory.GetFiles(dir))
            {
                string shortDir = Path.GetFileName(dir);
                Textures.Add(String.Join(".", shortDir, Path.GetFileName(fileDir)), Raylib.LoadTexture(fileDir));
            }
        }
    }

    public static Texture2D? TryGetTexture(string name)
    {
        if (Textures.ContainsKey(name))
        {
            return Textures[name];
        }

        return null;
    }

    public static Rectangle? GetNthSpriteRect(string name, int n)
    {
        TextureTiles.Textures.TryGetValue(name, out int TileCount);
        Textures.TryGetValue(name, out Texture2D Texture);

        float SpriteWidth = Texture.Width / TileCount;
        float SpriteHeight = Texture.Height;

        return new Rectangle(
                    (n-1) * SpriteWidth,
                    0,
                    SpriteWidth,
                    SpriteHeight 
                );
    }

    public static Rectangle GetTerrainTile(int i, int j)
    {
        float SpriteWidth = 16.0f;
        float SpriteHeight = 16.0f;

        return new Rectangle(j * SpriteWidth, i * SpriteHeight, SpriteWidth, SpriteHeight);
    }

    public static Rectangle GetTerrainTileByIndex(int spriteIndex)
    {
        float TextureWidth = 512.0f;
        float SpriteWidth = 16.0f;
        float SpriteHeight = 16.0f;

        int SpritesCol = (int) (TextureWidth / SpriteWidth);

        int i = spriteIndex / SpritesCol;
        int j = spriteIndex % SpritesCol;

        return new Rectangle(j * SpriteWidth, i * SpriteHeight, SpriteWidth, SpriteHeight);
    }
}
