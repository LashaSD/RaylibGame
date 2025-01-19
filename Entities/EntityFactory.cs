using Raylib_cs;

public static class EntityFactory
{
    private static Sprite? ConstructSprite(EntityData eData)
    {
        Texture2D? texture = TextureManager.TryGetTexture(eData.TextureFile);
        if (!texture.HasValue)
            return null;

        Rectangle? sourceRect = null;
        if (eData.Type == EntityType.Terrain && eData.TileMap is null)
            sourceRect = TextureManager.GetTerrainTileByIndex(eData.SpriteIndex ?? 0);
        else
        {
            sourceRect = TextureManager.GetNthSpriteRect(eData.TextureFile, 0);
            if (sourceRect is null)
                return null;
        }

        Sprite sprite = new(texture.Value, sourceRect.Value);
        sprite.SetScaleForSize(eData.Size);

        return sprite;
    }

    private static TileMap? ConstructTileMap(EntityData eData)
    {
        if (eData.TileMap is null)
            return null;

        Texture2D? texture = TextureManager.TryGetTexture(eData.TextureFile);
        if (!texture.HasValue)
            return null;

        TileMap tileMap = new(eData.Size);
        foreach (List<(int, int)> spriteRow in eData.TileMap)
        {
            List<Sprite> sprites = new();
            foreach ((int, int) spriteIdx in spriteRow)
            {
                for (int i = 0; i < spriteIdx.Item2; ++i)
                {
                    Sprite newSprite = new(texture.Value, TextureManager.GetTerrainTileByIndex(spriteIdx.Item1));
                    if (eData.Scale is not null)
                        newSprite.SetScale(eData.Scale.Value);
                    sprites.Add(newSprite);
                }
            }
            tileMap.AddSpriteRow(sprites);
        }

        return tileMap;
    }

    public static Entity ConstructStatic(EntityData eData)
    {
        Entity e = new();
        RenderComponent renderComponent = new();

        if (eData.Type == EntityType.Terrain && eData.TileMap is not null)
        {
            TileMap? tileMap = ConstructTileMap(eData);
            if (tileMap is null)
                throw new Exception("Couldn't Construct the Tile Map");

            renderComponent.SetTileMap(tileMap);
        }
        else
        {
            Sprite? sprite = ConstructSprite(eData);
            if (sprite is null)
                throw new Exception("Couldn't Construct A Sprite");

            renderComponent.SetSprite(sprite);
        }

        TransformComponent transform = new(eData.Position);

        if (eData.Scale is not null)
            transform.SetScale(eData.Scale.Value);

        StaticBodyComponent staticBody = new(eData.Size);
        if (eData.Friction is not null)
            staticBody.Friction = eData.Friction.Value;

        e.AddComponent<RenderComponent>(renderComponent);
        e.AddComponent<TransformComponent>(transform);
        e.AddComponent<StaticBodyComponent>(staticBody);

        e.Init();

        return e;
    }

    public static Entity ConstructDynamic(EntityData eData)
    {
        if (eData.Density is null)
            throw new Exception("Couldn't Construct A Dynamic Entity, No Density Was Provided");

        Sprite? sprite = ConstructSprite(eData);
        if (sprite is null)
            throw new Exception($"Couldn't Construct A Sprite {eData.TextureFile}");

        string? charType = eData.Type switch
        {
            EntityType.Player => "PlayerKnight",
            EntityType.Enemy => "EnemyKnight",
            _ => null,
        };

        if (charType is null)
            throw new Exception($"Invalid Entity Type for a Dynamic Body {eData.Type.ToString()}");

        Entity e = new();

        StateComponent state = new(charType);
        AnimationComponent anim = new();
        ActionComponent action = new();
        MovementComponent move = new();
        DynamicBodyComponent dynamicBody = new(eData.Size, eData.Density.Value);
        TransformComponent transform = new();
        transform.SetPosition(eData.Position);

        RenderComponent renderComponent = new();
        renderComponent.SetSprite(sprite);

        HealthComponent health = new();

        if (eData.Type == EntityType.Player)
        {
            InputComponent inputComponent = new();
            e.AddComponent<InputComponent>(inputComponent);

            CameraComponent camera = new();
            e.AddComponent<CameraComponent>(camera);
        }

        if (eData.Type == EntityType.Enemy)
        {
            AIComponent AiComponent = new();
            e.AddComponent<AIComponent>(AiComponent);
            // Custom Logic
        }

        e.AddComponent<StateComponent>(state);
        e.AddComponent<AnimationComponent>(anim);
        e.AddComponent<ActionComponent>(action);
        e.AddComponent<MovementComponent>(move);
        e.AddComponent<DynamicBodyComponent>(dynamicBody);
        e.AddComponent<TransformComponent>(transform);
        e.AddComponent<RenderComponent>(renderComponent);
        e.AddComponent<HealthComponent>(health);

        e.Init();

        if (eData.Type == EntityType.Player || eData.Type == EntityType.Enemy)
        {
            if (dynamicBody.PhysicsBody is not null)
                dynamicBody.PhysicsBody.FixedRotation = true;
        }

        return e;
    }
}
