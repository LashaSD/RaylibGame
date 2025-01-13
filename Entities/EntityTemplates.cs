using Raylib_cs;
using System.Numerics;

class EntityTemplates
{
    public static Entity PlayerEntity(Vector2 pos, Vector2 scale)
    {
        Entity PlayerEntity = new();
        PlayerEntity.AddComponent<StateComponent>(new StateComponent());
        PlayerEntity.AddComponent<AnimationComponent>(new AnimationComponent());
        PlayerEntity.AddComponent<ActionComponent>(new ActionComponent());
        PlayerEntity.AddComponent<InputComponent>(new InputComponent());
        PlayerEntity.AddComponent<DynamicBodyComponent>(new DynamicBodyComponent(new Vector2(72, 86), 80f));

        RenderComponent renderComponent = new();
        PlayerEntity.AddComponent<RenderComponent>(renderComponent);

        TransformComponent Transform = new();
        Transform.SetPosition(pos);
        PlayerEntity.AddComponent<TransformComponent>(Transform);

        PlayerEntity.Init();

        return PlayerEntity;
    }

    public static Entity KnightEntity(Vector2 pos)
    {
        Texture2D? knightTexture = TextureManager.TryGetTexture("Idle.png");

        Entity Knight = new();
        Knight.AddComponent<RenderComponent>(new RenderComponent());
        Knight.AddComponent<StateComponent>(new StateComponent());
        Knight.AddComponent<AnimationComponent>(new AnimationComponent());
        Knight.AddComponent<ActionComponent>(new ActionComponent());
        Knight.AddComponent<InputComponent>(new InputComponent());
        // Knight.AddComponent<DynamicBodyComponent>(new DynamicBodyComponent(25.0f));
        Knight.Init();

        return Knight;
    }

    public static Entity FloorEntity(Vector2 pos, Vector2 size)
    {
        Entity Floor = new();

        RenderComponent renderComponent = new RenderComponent();
        renderComponent.Sprite?.SetScaleForSize(new System.Numerics.Vector2(size.X, size.Y));
        Floor.AddComponent<RenderComponent>(renderComponent);

        Floor.AddComponent<StaticBodyComponent>(new StaticBodyComponent(size, 50.0f));
        TransformComponent transform = new();
        transform.SetPosition(pos);
        Floor.AddComponent<TransformComponent>(transform);
        Floor.Init();

        return Floor;
    }
}
