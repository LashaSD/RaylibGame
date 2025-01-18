using System.Numerics;
using Raylib_cs;

public enum EntityType
{
    Player,
    Enemy,
    Terrain,
    Construction
}

public struct EntityData
{
    public Vector2 Position;
    public Vector2 Size;
    public Vector2? Scale;

    public string TextureFile;

    // Grid of Sprite Segment Indices
    public List<List<(int, int)>>? TileMap;
    public int? SpriteIndex;

    public float? Friction;
    public float? Density;

    public int? SpriteRow;
    public int? SpriteCol;

    public EntityType Type;
}

public static class EntityFactory
{
    public static void ConstructStatic(EntityData eData)
    {
        Texture2D? texture = TextureManager.TryGetTexture(eData.TextureFile);
        if (!texture.HasValue)
        {
            Raylib.TraceLog(TraceLogLevel.Error, $"Failed to get Texture while constructing a static entity with Texture {eData.TextureFile}");
            return;
        }
        Entity e = new();
        RenderComponent renderComponent = new();

        Rectangle? sourceRect = null;
        if (eData.Type == EntityType.Terrain)
        {
            if (eData.TileMap is null)
            {
                sourceRect = TextureManager.GetTerrainTile(eData.SpriteRow ?? 0, eData.SpriteCol ?? 0);
                Sprite sprite = new(texture.Value, sourceRect.Value);
                sprite.SetScaleForSize(eData.Size);
                renderComponent.SetSprite(sprite);
            } else {
                TileMap tileMap = new(eData.Size);
                foreach (List<(int, int)> spriteRow in eData.TileMap)
                {
                    List<Sprite> sprites = new();
                    foreach ((int, int) spriteIdx in spriteRow)
                    {
                        for (int i = 0; i < spriteIdx.Item2; ++i)
                        {
                            Sprite newSprite = new(texture.Value, TextureManager.GetTerrainTileByIndex(spriteIdx.Item1));
                            if (eData.Scale is not null)
                                newSprite.SetScale(eData.Scale.Value);
                            sprites.Add(newSprite);
                        }
                    }
                    tileMap.AddSpriteRow(sprites);
                }

                renderComponent.SetTileMap(tileMap);
            }
        }
        else
        {
            sourceRect = TextureManager.GetNthSpriteRect(eData.TextureFile, 0);
            if (sourceRect is null)
                throw new Exception($"Couldn't get source rectangle of a texture: {eData.TextureFile}");

            Sprite sprite = new(texture.Value, sourceRect.Value);
            sprite.SetScaleForSize(eData.Size);
            renderComponent.SetSprite(sprite);
        }

        TransformComponent transform = new(eData.Position);
        StaticBodyComponent staticBody = new(eData.Size);
        if (eData.Friction is not null)
            staticBody.Friction = eData.Friction.Value;

        e.AddComponent<RenderComponent>(renderComponent);
        e.AddComponent<TransformComponent>(transform);
        e.AddComponent<StaticBodyComponent>(staticBody);

        e.Init();
    }

    public static void ConstructDynamic(EntityData eData)
    {
        if (eData.Density is null)
            return;

        Texture2D? texture = TextureManager.TryGetTexture(eData.TextureFile);
        Rectangle? srcRect = TextureManager.GetNthSpriteRect(eData.TextureFile, 0);
        if (texture is null || srcRect is null)
            return;

        Sprite sprite = new(texture.Value, srcRect.Value);

        Entity e = new();

        StateComponent state = new();
        AnimationComponent anim = new();
        ActionComponent action = new();
        MovementComponent move = new();
        DynamicBodyComponent dynamicBody = new(eData.Size, eData.Density.Value);
        TransformComponent transform = new();
        transform.SetPosition(eData.Position);

        RenderComponent renderComponent = new();
        renderComponent.SetSprite(sprite);

        if (eData.Type == EntityType.Player)
        {
            InputComponent inputComponent = new();
            e.AddComponent<InputComponent>(inputComponent);
        }

        if (eData.Type == EntityType.Enemy)
        {
            // Custom Logic
        }

        e.AddComponent<StateComponent>(state);
        e.AddComponent<AnimationComponent>(anim);
        e.AddComponent<ActionComponent>(action);
        e.AddComponent<MovementComponent>(move);
        e.AddComponent<DynamicBodyComponent>(dynamicBody);
        e.AddComponent<TransformComponent>(transform);
        e.AddComponent<RenderComponent>(renderComponent);

        e.Init();
    }
}

public class GameWorld
{
    private List<EntityData> StaticBodies = new();
    private List<EntityData> DynamicBodies = new();

    public void AddStaticBodies(params EntityData[] data)
    {
        this.StaticBodies.AddRange(data);
    }

    public void AddDynamicBodies(params EntityData[] data)
    {
        this.DynamicBodies.AddRange(data);
    }

    public void Construct()
    {
        foreach (EntityData eData in this.StaticBodies)
            EntityFactory.ConstructStatic(eData);

        foreach (EntityData eData in this.DynamicBodies)
            EntityFactory.ConstructDynamic(eData);
    }
}

public static class WorldReader
{
    const int WindowWidth = 1600;
    const int WindowHeight = 900;

    public static GameWorld ReadFile()
    { 
        GameWorld world = new();
        world.AddStaticBodies(new EntityData() { 
            Position = new Vector2(2400 / 2, WindowHeight - 50),
            TileMap = new List<List<(int, int)>>() { 
                new List<(int, int)>() { (929, 10), (928, 40) },
                new List<(int, int)>() { (961, 50 )},
                new List<(int, int)>() { (961, 50 )},
                new List<(int, int)>() { (961, 50 )},
                new List<(int, int)>() { (993, 50 )},
                new List<(int, int)>() { (993, 50 )},
                new List<(int, int)>() { (993, 50 )},
            },
            TextureFile = "TerrainTexture.png",
            Size = new Vector2(2400, 100),
            Scale = new Vector2(3, 3),
            Type = EntityType.Terrain,
            Friction = 250 
        });

        world.AddDynamicBodies(
                new EntityData() {
                    Density = 80,
                    Position = new Vector2(200, 100),
                    Size = new Vector2(80, 86),
                    TextureFile = "Idle.png",
                    Type = EntityType.Player
                }
        );

        return world; 
    }
}
