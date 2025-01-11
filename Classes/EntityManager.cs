public class EntityManager 
{
    private int Id = 0;
    public readonly List<Entity> Entities = new();

    public List<Entity> GetEntitiesWithComponent<T>() where T : Component
    {
        List<Entity> EntitiesWithComponent = new();

        foreach (Entity e in this.Entities)
        {
            if (e.TryGetComponent<T>() is not null)
            {
                EntitiesWithComponent.Add(e);
            }
        }

        return EntitiesWithComponent;
    }

    public void AddEntity(Entity entity) 
    {
        entity.Id = this.Id++;
        this.Entities.Add(entity);
    }
}
