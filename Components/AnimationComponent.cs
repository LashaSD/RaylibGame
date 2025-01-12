public class AnimationComponent : Component
{
    public Animation? CurrentAnimation;
    
    public AnimationComponent()
    {
        AnimationSystem.Register(this);
    }

    public void LoadAnim(Animation animation)
    {
        this.CurrentAnimation?.Stop();
        this.CurrentAnimation = animation;
    }

    public override void Update(float deltaTime)
    {
        if (this.CurrentAnimation is null)
            return;

        this.CurrentAnimation.Update(deltaTime);
        RenderComponent? renderComponent = this.ParentEntity?.GetComponent<RenderComponent>();
        if (renderComponent is not null)
        {
            renderComponent.Texture = this.CurrentAnimation.AnimKeyFrames.Texture;
            renderComponent.SourceRect = this.CurrentAnimation.AnimKeyFrames.SpriteRect;
        }
    }
}
