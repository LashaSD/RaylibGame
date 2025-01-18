using Raylib_cs;
using System.Numerics;

public class CharacterAnimationSettings
{
    public Dictionary<State, string> AnimationMap { get; set; } = new();
}

public static class Settings
{
    public static readonly Dictionary<KeyboardKey, IAction> ActionKeybinds = new Dictionary<KeyboardKey, IAction>()
    {
        { KeyboardKey.Space, new JumpAction() },
    };

    public static readonly Dictionary<KeyboardKey, Vector2> MovementKeybinds = new Dictionary<KeyboardKey, Vector2>()
    {
        { KeyboardKey.D, new Vector2(1, 0) },
        { KeyboardKey.A, new Vector2(-1, 0) },
    };

//     public static readonly Dictionary<string, CharacterAnimationSettings> CharacterSettings =
//         new Dictionary<string, CharacterAnimationSettings>
//         {
//             ["PlayerKnight"] = new CharacterAnimationSettings
//             {
//                 AnimationMap = new Dictionary<State, Animation>
//                 {
//                     { State.Idle, "PlayerKnight" },
//                     { State.Run, "PlayerRunAnimation" },
//                     { State.Jump, "PlayerJumpAnimation" },
//                 }
//             },
//             ["Enemy"] = new CharacterAnimationSettings
//             {
//                 AnimationMap = new Dictionary<State, string>
//                 {
//                     { State.Idle, "EnemyIdleAnimation" },
//                     { State.Run, "EnemyRunAnimation" },
//                     { State.Jump, "EnemyJumpAnimation" },
//                 }
//             }
//         };
}
