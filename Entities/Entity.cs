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

    public void Init()
    {
        foreach (Component component in this.Components.Values)
            component.Init();
    }

    public void Update(float deltaTime)
    {
        foreach (Component component in this.Components.Values)
            component.Update(deltaTime);
    }
}
