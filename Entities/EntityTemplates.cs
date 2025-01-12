using Raylib_cs;
using Microsoft.Xna.Framework;

class EntityTemplates
{
    public static Entity PlayerEntity(Vector2 pos, Vector2 size)
    {
        Entity PlayerEntity = new();
        PlayerEntity.AddComponent<RenderComponent>(new RenderComponent());
        PlayerEntity.AddComponent<StateComponent>(new StateComponent());
        PlayerEntity.AddComponent<AnimationComponent>(new AnimationComponent());
        PlayerEntity.AddComponent<ActionComponent>(new ActionComponent());
        PlayerEntity.AddComponent<InputComponent>(new InputComponent());
        PlayerEntity.AddComponent<DynamicBodyComponent>(new DynamicBodyComponent(pos, size, 800.0f));

        return PlayerEntity;
    }

    public static Entity FloorEntity(Vector2 pos, Vector2 size)
    {
        Entity Floor = new();

        Floor.AddComponent<RenderComponent>(new RenderComponent());
        Floor.AddComponent<TransformComponent>(new TransformComponent() { 
            Pos = pos,
        });

        Floor.AddComponent<StaticBodyComponent>(new StaticBodyComponent(pos, size, 50.0f));

        return Floor;
    }
}
