using Raylib_cs;
using System.IO;

class TextureManager
{
    Dictionary<string, Texture2D> Textures = new();
    
    public void LoadTextures()
    {
        string baseDirectory = Environment.CurrentDirectory;
        string folderName = "Assets";
        string directoryPath = Path.Join(baseDirectory, folderName);

        string[] files = Directory.GetFiles(directoryPath);
        string[] directories = Directory.GetDirectories(directoryPath);

        foreach (string fileDir in files)
        {
            this.Textures.Add(Path.GetFileName(fileDir), Raylib.LoadTexture(fileDir));
        }

        foreach (string dir in directories)
        {
            foreach (string fileDir in Directory.GetFiles(dir))
            {
                this.Textures.Add(Path.GetFileName(fileDir), Raylib.LoadTexture(fileDir));
            }
        }
    }

    public Texture2D? TryGetTexture(string name)
    {
        if (this.Textures.ContainsKey(name))
        {
            return this.Textures[name];
        }

        return null;
    }

    public Rectangle GetNthSpriteRect(string name, int n)
    {
        int TileCount;
        TextureTiles.Textures.TryGetValue(name, out TileCount);

        Texture2D Texture;
        this.Textures.TryGetValue(name, out Texture);

        float SpriteWidth = Texture.Width / TileCount;
        float SpriteHeight = Texture.Height;

        return new Rectangle(
                    (n-1) * SpriteWidth,
                    0,
                    SpriteWidth,
                    SpriteHeight 
                );
    }
}
