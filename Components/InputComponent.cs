using Raylib_cs;
using System.Numerics;

public class InputComponent : Component
{
    private ActionComponent? ActionComponent;
    private MovementComponent? MovementComponent;

    public InputComponent()
    {
        InputSystem.Register(this);
    }

    public override void Init()
    {
        if (this.ParentEntity is null)
            return;

        this.ActionComponent = this.ParentEntity.GetComponent<ActionComponent>();
        this.MovementComponent = this.ParentEntity.GetComponent<MovementComponent>();
    }

    public override void Update(float deltaTime)
    {
        if (this.ParentEntity is null)
            return;

        if (this.ActionComponent is not null)
        {
            foreach (KeyboardKey key in Settings.ActionKeybinds.Keys)
            {
                if (Raylib.IsKeyDown(key))
                {
                    this.ActionComponent.Execute(Settings.ActionKeybinds[key]);
                }
            }
        }

        if (this.MovementComponent is not null)
        {
            Vector2 direction = Vector2.Zero; 
            foreach (KeyboardKey key in Settings.MovementKeybinds.Keys)
            {
                if (Raylib.IsKeyDown(key))
                    direction += Settings.MovementKeybinds[key];
            }

            MovementComponent.Update(deltaTime, direction);
        }
    }
}
