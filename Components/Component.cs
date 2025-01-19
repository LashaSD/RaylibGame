public abstract class Component
{
    public Entity? ParentEntity {get; private set;}

    // Initialized only when the Component is added to an Entity
    public virtual void Init()
    {}

    public virtual void Destroy()
    {}

    public virtual void Update(float deltaTime)
    {}

    internal void SetEntity(Entity entity)
    {
        this.ParentEntity = entity;
    }
}
