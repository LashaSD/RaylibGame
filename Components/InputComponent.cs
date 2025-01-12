using Raylib_cs;

public class InputComponent : Component
{
    public InputComponent()
    {
        InputSystem.Register(this);
    }

    public override void Update(float deltaTime)
    {
        if (this.ParentEntity is null)
            return;

        ActionComponent? actionComponent = this.ParentEntity.GetComponent<ActionComponent>();
        if (actionComponent is null)
            return;

        foreach (KeyboardKey key in Settings.Keybinds.Keys)
        {
            if (Raylib.IsKeyDown(key))
            {
                actionComponent.Execute(Settings.Keybinds[key]);
            }
        }
    }
}
