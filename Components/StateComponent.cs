using Raylib_cs;

public enum State
{
    Attack1,
    Attack2,
    Attack3,
    Dead,
    Defend,
    Hurt,
    Idle,
    Jump,
    Protect,
    Run,
    Run_Attack,
    Walk
}

public class StateComponent : Component
{
    public Signal<State> StateChanged = new Signal<State>();
    public State CurrentState { get; private set; }

    public void SetState(State newState)
    {
        this.CurrentState = newState;
        this.OnStateChanged(newState);
        this.StateChanged.Fire(newState);
    }

    public StateComponent()
    {
        StateSystem.Register(this);
    }

    public override void Init()
    {
        this.SetState(State.Idle);
        this.OnStateChanged(State.Idle);
    }

    private void OnStateChanged(State newState) 
    {
        if (this.ParentEntity is null)
            return;

        AnimationComponent? animComponent = this.ParentEntity.GetComponent<AnimationComponent>();

        if (animComponent is null)
            return;

        Animation anim = MapStateToAnim(newState);
        animComponent.LoadAnim(anim);
        anim.Play();
        Raylib.TraceLog(TraceLogLevel.Info, $"{newState}");
    }

    private Animation MapStateToAnim(State state)
    {
        Texture2D? textureDefault = TextureManager.TryGetTexture("Idle.png");
        Rectangle? spriteRectDefault = TextureManager.GetNthSpriteRect($"Idle.png", 1);

        if (textureDefault is null || spriteRectDefault is null)
            throw new Exception("Failed to fetch texture Idle.png");

        Texture2D? texture = TextureManager.TryGetTexture($"{state.ToString()}.png");
        Rectangle? spriteRect = TextureManager.GetNthSpriteRect($"{state.ToString()}.png", 1);

        if (texture is null || spriteRect is null)
            throw new Exception($"Failed to fetch texture {state}");

        return state switch
        {
            State.Attack1 => new Animation(new KeyFrames((Texture2D) texture, (Rectangle) spriteRect), 2.5f),
            State.Run => new Animation(new KeyFrames((Texture2D) texture, (Rectangle) spriteRect), 1.5f, true),
            State.Idle => new Animation(new KeyFrames((Texture2D)texture, (Rectangle) spriteRect), 0.01f),
            State.Jump => new Animation(new KeyFrames((Texture2D)texture, (Rectangle) spriteRect), 1.25f),
            State.Run_Attack => new Animation(new KeyFrames((Texture2D)texture, (Rectangle) spriteRect), 2.5f, true),
            _ => new Animation(new KeyFrames((Texture2D)textureDefault, (Rectangle) spriteRectDefault), 2.5f),
        };
    }

}
