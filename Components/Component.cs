public abstract class Component
{
    public Entity? ParentEntity {get; private set;}

    public abstract void Init();
    public abstract void Update(float deltaTime);

    internal void SetEntity(Entity entity)
    {
        this.ParentEntity = entity;
    }
}
