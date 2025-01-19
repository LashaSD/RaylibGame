using System.Text.Json.Serialization;
using System.Numerics;

public enum EntityType
{
    Player,
    Enemy,
    Terrain
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

public class Entity
{
    public int Id { get; set; }

    private readonly Dictionary<Type, Component> Components = new Dictionary<Type, Component>();

    public void AddComponent<T>(T component) where T : Component
    {
        if (!this.Components.ContainsKey(typeof(T)))
        {
            component.SetEntity(this);
            this.Components.Add(typeof(T), component);
        }
    }

    public T? GetComponent<T>() where T : Component
    {
        if (!this.Components.ContainsKey(typeof(T)))
        {
            return null;
        }
        return (T) this.Components[typeof(T)];
    }

    public void RemoveComponent<T>() where T : Component
    {
        var component = this.GetComponent<T>();
        if (component != null)
        {
            component.Destroy();
            this.Components.Remove(typeof(T));
        }
    }

    public void Init()
    {
        foreach (Component component in this.Components.Values)
            component.Init();
    }

    public void Destroy()
    {
        // Unregister components from their systems and clean up
        foreach (var component in Components.Values)
        {
            component.Destroy(); // Ensure the component cleans itself up
        }
        Components.Clear();
    }

    public void Update(float deltaTime)
    {
        foreach (Component component in this.Components.Values)
            component.Update(deltaTime);
    }
}
