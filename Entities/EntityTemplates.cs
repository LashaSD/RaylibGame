using Raylib_cs;
using System.Numerics;

class EntityTemplates
{
    public static Entity PlayerEntity(Texture2D texture)
    {
        Entity PlayerEntity = new();
        PlayerEntity.AddComponent<RenderComponent>(new RenderComponent() { 
                    Texture = texture,
                    SourceRect = new Rectangle(70, 0, 70, 86),
                    Origin = new Vector2(0, 0),
                });

        PlayerEntity.AddComponent<TransformComponent>(new TransformComponent() { 
                    Position = new Vector2(0, 0),
                    Size = new Vector2(70 * 4, 86 * 4)
                });

        return PlayerEntity;
    }

    public static Entity PlayerEntity(Texture2D texture, Vector2 position)
    {
        Entity PlayerEntity = new();
        PlayerEntity.AddComponent<RenderComponent>(new RenderComponent() { 
                    Texture = texture,
                    SourceRect = new Rectangle(70, 0, 70, 86),
                    Origin = new Vector2(0, 0),
                });

        PlayerEntity.AddComponent<TransformComponent>(new TransformComponent() { 
                    Position = position,
                    Size = new Vector2(70 * 4, 86 * 4)
                });

        return PlayerEntity;
    }
}
