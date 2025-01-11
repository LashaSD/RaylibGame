public class Entity
{
    public int Id { get; set; }

    private readonly Dictionary<Type, Component> Components = new Dictionary<Type, Component>();

    public void AddComponent<T>(T component) where T : Component
    {
        if (!this.Components.ContainsKey(typeof(T)))
        {
            component.SetEntity(this);
            component.Init();
            this.Components.Add(typeof(T), component);
        }
    }

    public T? TryGetComponent<T>() where T : Component
    {
        return this.Components.TryGetValue(typeof(T), out var component) ? (T)component : null;
    }

    public void Update(float deltaTime)
    {
        foreach (Component component in this.Components.Values)
        {
            component.Update(deltaTime);
        }
    }
}
