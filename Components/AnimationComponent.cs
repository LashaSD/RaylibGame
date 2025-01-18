public class AnimationComponent : Component
{
    private RenderComponent? RenderComponent;

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

    public override void Init()
    {
        this.RenderComponent = this.ParentEntity?.GetComponent<RenderComponent>();
    }

    public override void Update(float deltaTime)
    {
        if (this.CurrentAnimation is null)
            return;

        this.CurrentAnimation.Update(deltaTime);

        Sprite sprite = new Sprite(
            this.CurrentAnimation.AnimKeyFrames.Texture,
            this.CurrentAnimation.AnimKeyFrames.SpriteRect
        );

        this.RenderComponent?.SetSprite(sprite);
    }
}
