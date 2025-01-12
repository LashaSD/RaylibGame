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
                Textures.Add(Path.GetFileName(fileDir), Raylib.LoadTexture(fileDir));
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
        int TileCount;
        TextureTiles.Textures.TryGetValue(name, out TileCount);

        Texture2D Texture;
        Textures.TryGetValue(name, out Texture);

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

        return new Rectangle(i * SpriteWidth, j * SpriteHeight, SpriteWidth, SpriteHeight);
    }
}
