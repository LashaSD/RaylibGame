using Raylib_cs;
using System.Numerics;

public class CameraComponent : Component
{
    public Camera2D Camera = new();

    private TransformComponent? Transform;
    private MovementComponent? MovementComponent;
    private RenderComponent? RenderComponent;

    public float Smoothness { get; set; } = 0.15f;
    public float Zoom { get; set; } = 1.5f;
    public float DirectionConstantPx { get; set; } = 50;
    public float HeightScale { get; set; } = 0.65f;

    public CameraComponent()
    {
        CameraSystem.Register(this);
    }

    public override void Init()
    {
        this.Transform = this.ParentEntity?.GetComponent<TransformComponent>();
        this.MovementComponent = this.ParentEntity?.GetComponent<MovementComponent>();
        this.RenderComponent = this.ParentEntity?.GetComponent<RenderComponent>();

        this.Camera.Zoom = this.Zoom;
    }

    public override void Destroy()
    {
        CameraSystem.Remove(this);
    }

    public override void Update(float deltaTime)
    {
        if (this.Transform is null || this.RenderComponent is null || this.RenderComponent.Sprite is null)
            return;

        Vector2 offset = new Vector2(0, 0);
        if (this.MovementComponent is not null)
            offset = this.MovementComponent.MoveDirection * this.DirectionConstantPx;

        Vector2 targetPosition = (this.Transform.Position + this.RenderComponent.Sprite.SizePx / 2) + offset;

        this.Camera.Target = Vector2.Lerp(this.Camera.Target, targetPosition, this.Smoothness);
        this.Camera.Offset = new Vector2(Raylib.GetScreenWidth() / 2, Raylib.GetScreenHeight() * this.HeightScale);
        this.Camera.Zoom = this.Zoom;
    }
}
