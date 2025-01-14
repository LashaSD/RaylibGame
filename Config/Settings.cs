using Raylib_cs;
using System.Numerics;

public static class Settings
{
    public static Dictionary<KeyboardKey, IAction> ActionKeybinds = new Dictionary<KeyboardKey, IAction>()
    {
        { KeyboardKey.Space, new JumpAction() },
    };

    public static Dictionary<KeyboardKey, Vector2> MovementKeybinds = new Dictionary<KeyboardKey, Vector2>()
    {
        { KeyboardKey.D, new Vector2(1, 0) },
        { KeyboardKey.A, new Vector2(-1, 0) },
    };
}
