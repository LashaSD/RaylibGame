using Raylib_cs;

class AnimationSystem
{
    private EntityManager entityManager;
    private TextureManager textureManager;

    public int frameIndex = 1;

    public AnimationSystem(EntityManager entityManager, TextureManager textureManager)
    {
        this.entityManager = entityManager;
        this.textureManager = textureManager;
    }

    public void Update(float deltaTime)
    {
        List<Entity> entitiesToUpdate = this.entityManager.GetEntitiesWithComponent<StateComponent>();
        
        foreach (Entity entity in entitiesToUpdate)
        {
            StateComponent? stateComponent = entity.TryGetComponent<StateComponent>();
            RenderComponent? renderComponent = entity.TryGetComponent<RenderComponent>();

            if (renderComponent is null || stateComponent is null)
                continue;
            
            string textureName = $"{stateComponent.CurrentState.ToString()}.png";
            Texture2D? texture = this.textureManager.TryGetTexture(textureName);

            if (texture is null)
                continue;

            renderComponent.Texture = (Texture2D) texture;
            renderComponent.SourceRect = this.textureManager.GetNthSpriteRect(textureName, this.frameIndex);
        }
    }
}
