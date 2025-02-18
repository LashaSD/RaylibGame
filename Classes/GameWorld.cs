using System.Text.Json;

public class GameWorld
{
    public readonly Dictionary<int, Entity> EntityMap = new();
    public readonly List<Entity> Killables = new();

    public int PlayerId { get; private set; } = -1;
    private int IdTracker = 0;

    public void Construct(EntityDataContainer data)
    {
        foreach (EntityData eData in data.StaticEntities)
            this.SpawnStatic(eData);

        foreach (EntityData eData in data.DynamicEntities)
            this.SpawnDynamic(eData);
    }

    public void SpawnDynamic(EntityData data)
    {
        Entity e = EntityFactory.ConstructDynamic(data);
        e.Id = this.IdTracker++;
        this.EntityMap.Add(e.Id, e);

        if (data.Type == EntityType.Player || data.Type == EntityType.Enemy)
            this.Killables.Add(e);

        if (data.Type == EntityType.Player)
            this.PlayerId = e.Id;
    }

    public void SpawnStatic(EntityData data)
    {
        Entity e = EntityFactory.ConstructStatic(data);
        e.Id = this.IdTracker++;
        this.EntityMap.Add(e.Id, e);
    }

    public void Heartbeat()
    {
        foreach (Entity killable in this.Killables)
        {
            if (killable.DeathMark)
            {
                killable.Destroy();
            }
        }
    }
}

public static class WorldReader
{
    const int WindowWidth = 1600;
    const int WindowHeight = 900;

    public static EntityDataContainer AssignJsonData(string filePath){
        var options = new JsonSerializerOptions
        {
            PropertyNameCaseInsensitive = true,
            Converters =
            {
                new Vector2JsonConverter(),
                new TileMapJsonConverter()
            }
        };

        var jsonFile = File.ReadAllText(Path.Join(PathHelper.GetProjectDirectory(), filePath));
        return JsonSerializer.Deserialize<EntityDataContainer>(jsonFile, options) ?? throw new InvalidOperationException("Failed to parse JSON.");;
    }

    public static EntityDataContainer ReadFile(string filePath)
    { 
        EntityDataContainer DataContainer = AssignJsonData(filePath);
        return DataContainer; 
    }
}
