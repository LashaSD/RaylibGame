using System.Numerics;

public interface IAction
{
    public void Execute(Entity entity);
}

public class JumpAction : IAction
{
    public void Execute(Entity entity)
    {
        TransformComponent? transform = entity.GetComponent<TransformComponent>();

        if (transform is null)
            return;

        Vector2 pos = transform.Position;
        transform.SetPos(new Vector2(pos.X, pos.Y + 15.0f));
    }
}

public class ActionComponent : Component
{
    public void Execute(IAction action)
    {
        if (this.ParentEntity is not null)
            action.Execute(this.ParentEntity);
    }
}
