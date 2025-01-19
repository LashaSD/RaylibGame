public class AnimationComponent : Component
{
    private RenderComponent? RenderComponent;
    private StateComponent? StateComponent;

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
        this.StateComponent = this.ParentEntity?.GetComponent<StateComponent>();
    }

    public override void Destroy()
    {
        AnimationSystem.Remove(this);
    }

    public override void Update(float deltaTime)
    {
        if (this.CurrentAnimation is null)
            return;

        if (this.CurrentAnimation.AnimState == AnimationState.Finished || this.CurrentAnimation.AnimState == AnimationState.Cancelled)
        {
            if (this.StateComponent?.CurrentState != State.Defend)
            {
                // this.StateComponent?.SetState(State.Idle);
                return;
            }
        }

        this.CurrentAnimation.Update(deltaTime);

        Sprite sprite = new Sprite(
            this.CurrentAnimation.AnimKeyFrames.Texture,
            this.CurrentAnimation.AnimKeyFrames.SpriteRect
        );

        this.RenderComponent?.SetSprite(sprite);
    }
}
