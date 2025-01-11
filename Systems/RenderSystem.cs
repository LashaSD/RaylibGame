using Raylib_cs;

class RenderSystem
{
    private EntityManager entityManager = new();

    public RenderSystem(EntityManager entityManager)
    {
        this.entityManager = entityManager;
    }

    public void Render()
    {
        List<Entity> entitiesToRender = this.entityManager.GetEntitiesWithComponent<RenderComponent>();
        
        // Sort by Z-Index if needed
        var sortedEntities = entitiesToRender
            .OrderBy(e => {
                    RenderComponent? renderComponent = e.TryGetComponent<RenderComponent>();
                    if (renderComponent is not null)
                    return renderComponent.ZIndex;

                    return 9999999;
                });

        foreach (Entity entity in sortedEntities)
        {
            RenderComponent? renderComponent = entity.TryGetComponent<RenderComponent>();
            TransformComponent? transformComponent = entity.TryGetComponent<TransformComponent>();

            if (renderComponent is null || transformComponent is null)
                continue;

            if (!renderComponent.Visible) 
                continue;

            // ToDo:
            // - [ ] Possible Optimization:
            // If the Position of the Entiry is outside of the Camera's Viewport then don't render

            this.Draw(renderComponent, transformComponent);
        }
    }

    private void Draw(RenderComponent renderComponent, TransformComponent transform)
    {
        if (renderComponent.Shader != null && renderComponent.Shader is not null)
        {
            Raylib.BeginShaderMode((Shader) renderComponent.Shader);
        }

        Raylib.DrawTexturePro(
            renderComponent.Texture,
            renderComponent.SourceRect,
            new Rectangle(transform.Position.X, transform.Position.Y, transform.Size.X, transform.Size.Y),
            renderComponent.Origin,
            renderComponent.Rotation,
            Color.White
        );

        if (renderComponent.Shader != null && renderComponent.Shader is not null)
        {
            Raylib.EndShaderMode();
        }
    }
}

