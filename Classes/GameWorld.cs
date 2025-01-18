using System.Numerics;
using Raylib_cs;
using System.Text.Json;
using System.Text.Json.Serialization;

public enum EntityType
{
    Player,
    Enemy,
    Terrain,
    Construction
}

public struct EntityData
{
    [JsonPropertyName("TextureFile")]
    public string TextureFile { get; set; }

    [JsonPropertyName("TileMap")]
    public List<List<(int, int)>>? TileMap { get; set; }

    [JsonPropertyName("Position")]
    public Vector2 Position { get; set; }

    [JsonPropertyName("Size")]
    public Vector2 Size { get; set; }

    [JsonPropertyName("Scale")]
    public Vector2? Scale { get; set; }

    [JsonPropertyName("Friction")]
    public float? Friction { get; set; }

    [JsonPropertyName("Density")]
    public float? Density { get; set; }

    [JsonPropertyName("SpriteIndex")]
    public int? SpriteIndex { get; set; }

    [JsonPropertyName("Type")]
    public EntityType Type { get; set; }
}

public class EntityDataContainer
{
    public List<EntityData> StaticEntities { get; set; } = new();
    public List<EntityData> DynamicEntities { get; set; } = new();
}

public static class EntityFactory
{
    private static Sprite? ConstructSprite(EntityData eData)
    {
        Texture2D? texture = TextureManager.TryGetTexture(eData.TextureFile);
        if (!texture.HasValue)
            return null;

        Rectangle? sourceRect = null;
        if (eData.Type == EntityType.Terrain && eData.TileMap is null)
            sourceRect = TextureManager.GetTerrainTileByIndex(eData.SpriteIndex ?? 0);
        else
        {
            sourceRect = TextureManager.GetNthSpriteRect(eData.TextureFile, 0);
            if (sourceRect is null)
                return null;
        }

        Sprite sprite = new(texture.Value, sourceRect.Value);
        sprite.SetScaleForSize(eData.Size);

        return sprite;
    }

    private static TileMap? ConstructTileMap(EntityData eData)
    {
        if (eData.TileMap is null)
            return null;

        Texture2D? texture = TextureManager.TryGetTexture(eData.TextureFile);
        if (!texture.HasValue)
            return null;

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

        return tileMap;
    }

    public static void ConstructStatic(EntityData eData)
    {
        Entity e = new();
        RenderComponent renderComponent = new();

        if (eData.Type == EntityType.Terrain && eData.TileMap is not null)
        {
            TileMap? tileMap = ConstructTileMap(eData);
            if (tileMap is null)
                throw new Exception("Couldn't Construct the Tile Map");

            renderComponent.SetTileMap(tileMap);
        }
        else
        {
            Sprite? sprite = ConstructSprite(eData);
            if (sprite is null)
                throw new Exception("Couldn't Construct A Sprite");

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

        Sprite? sprite = ConstructSprite(eData);
        if (sprite is null)
            throw new Exception($"Couldn't Construct A Sprite {eData.TextureFile}");

        string? charType = eData.Type switch
        {
            EntityType.Player => "PlayerKnight",
            EntityType.Enemy => "EnemyKnight",
            _ => null,
        };

        if (charType is null)
            throw new Exception($"Invalid Entity Type for a Dynamic Body {eData.Type.ToString()}");

        Entity e = new();

        StateComponent state = new(charType);
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
            AIComponent AiComponent = new();
            e.AddComponent<AIComponent>(AiComponent);
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

        if (eData.Type == EntityType.Player)
        {
            if (dynamicBody.PhysicsBody is not null)
                dynamicBody.PhysicsBody.FixedRotation = true;
        }
    }
}

public class GameWorld
{
    private List<EntityData> StaticBodies = new();
    private List<EntityData> DynamicBodies = new();

    public void AddStaticEntities(List<EntityData> data)
    {
        this.StaticBodies.AddRange(data);
    }

    public void AddDynamicEntities(List<EntityData> data)
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

    public static EntityDataContainer AssignJsonData(string json){
        var options = new JsonSerializerOptions
        {
            PropertyNameCaseInsensitive = true,
            Converters =
            {
                new Vector2JsonConverter(),
                new TileMapJsonConverter()
            }
        };

        var jsonFile = File.ReadAllText(json);
        return JsonSerializer.Deserialize<EntityDataContainer>(jsonFile, options) ?? throw new InvalidOperationException("Failed to parse JSON.");;
    }

    public static GameWorld ReadFile(string json)
    { 
        EntityDataContainer JsonData = AssignJsonData(json);
        GameWorld world = new();

        world.AddStaticEntities(JsonData.StaticEntities);
        world.AddDynamicEntities(JsonData.DynamicEntities);

        return world; 
    }
}
