using Raylib_cs;

public static class Settings
{
    public static Dictionary<KeyboardKey, IAction> Keybinds = new Dictionary<KeyboardKey, IAction>()
    {
        { KeyboardKey.Space, new JumpAction() },
    };    
}
