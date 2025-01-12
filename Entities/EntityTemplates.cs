using Raylib_cs;
using System.Numerics;

class EntityTemplates
{
    public static Entity PlayerEntity()
    {
        Entity PlayerEntity = new();
        PlayerEntity.AddComponent<RenderComponent>(new RenderComponent());
        PlayerEntity.AddComponent<TransformComponent>(new TransformComponent());
        PlayerEntity.AddComponent<StateComponent>(new StateComponent());
        PlayerEntity.AddComponent<AnimationComponent>(new AnimationComponent());
        PlayerEntity.AddComponent<ActionComponent>(new ActionComponent());
        PlayerEntity.AddComponent<InputComponent>(new InputComponent());

        return PlayerEntity;
    }
}
