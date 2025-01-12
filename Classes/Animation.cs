using Raylib_cs;

public enum AnimationState
{
    Init,
    Starting,
    Playing,
    Paused,
    Finished,
    Cancelled
}

public class Animation
{
    public KeyFrames AnimKeyFrames { get; set; }

    public float AnimStartTime { get; private set; }
    public float Duration { get; private set; }

    public bool Loop { get; set; } = false;
    public AnimationState AnimState { get; private set; } = AnimationState.Init;

    public Animation(KeyFrames keyFrames, float duration)
    {
        this.AnimKeyFrames = keyFrames;
        this.Duration = duration;
        this.AnimState = AnimationState.Starting;
    }

    public Animation(KeyFrames keyFrames, float duration, bool loop)
        : this(keyFrames, duration)
    {
        this.Loop = loop;
    }

    public void Play()
    {
        if (AnimKeyFrames is null)
            return;

        this.AnimStartTime = (float) Raylib.GetTime();
        this.AnimState = AnimationState.Playing;
    }

    public void Stop()
    {
        if (AnimKeyFrames is null)
            return;

        this.AnimState = AnimationState.Cancelled;
    }

    public void Update(float deltaTime)
    {
        if (this.AnimKeyFrames is null || this.AnimState == AnimationState.Finished || this.AnimState == AnimationState.Cancelled)
            return;

        int spriteCnt = (int) (this.AnimKeyFrames.Texture.Width / this.AnimKeyFrames.SpriteRect.Width);
        float spriteDuration = this.Duration / spriteCnt;
        int spriteIndex = (int) (this.AnimKeyFrames.SpriteRect.X / this.AnimKeyFrames.SpriteRect.Width);

        float timeElapsed = (float) Raylib.GetTime() - this.AnimStartTime;
        // the sprite index that the animation should be at this time
        int currentSpriteIndex = (int) (timeElapsed / spriteDuration);

        if (currentSpriteIndex >= spriteCnt && !this.Loop)
        {
            this.AnimState = AnimationState.Finished;
            return;
        }

        if (this.Loop)
            currentSpriteIndex %= spriteCnt;

        if (currentSpriteIndex != spriteIndex)
        {
            if (this.Loop)
                this.AnimKeyFrames.NextCyclic();
            else
                this.AnimKeyFrames.Next();
        }
    }
}
