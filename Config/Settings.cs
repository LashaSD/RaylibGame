using Raylib_cs;
using System.Numerics;

public class CharacterAnimationSettings
{
    public Dictionary<State, AnimationData> AnimationMap { get; set; } = new();
}

public struct AnimationData 
{
    public string TextureName { get; set; }
    public bool Loop { get; set; }
    public float Duration { get; set; }
}

public static class Settings
{
    public static readonly Dictionary<KeyboardKey, IAction> ActionKeybinds = new Dictionary<KeyboardKey, IAction>()
    {
        { KeyboardKey.Space, new JumpAction() },
        { KeyboardKey.F, new AttackAction1() },
    };

    public static readonly Dictionary<KeyboardKey, Vector2> MovementKeybinds = new Dictionary<KeyboardKey, Vector2>()
    {
        { KeyboardKey.D, new Vector2(1, 0) },
        { KeyboardKey.A, new Vector2(-1, 0) },
    };

    public static readonly Dictionary<string, CharacterAnimationSettings> CharacterSettings =
        new Dictionary<string, CharacterAnimationSettings>
        {
            ["PlayerKnight"] = new CharacterAnimationSettings
            {
                AnimationMap = new Dictionary<State, AnimationData>
                {
                    { State.Idle, AnimationHelper.QuickAnimData("PlayerKnight.Idle.png", 0.01f, false) },
                    { State.Run, AnimationHelper.QuickAnimData("PlayerKnight.Run.png", 1.25f, true) },
                    { State.Jump, AnimationHelper.QuickAnimData("PlayerKnight.Jump.png", 1.13f, false) },
                    { State.Attack1, AnimationHelper.QuickAnimData("PlayerKnight.Attack2.png", 0.23f, false) }
                }
            },
            ["EnemyKnight"] = new CharacterAnimationSettings
            {
                AnimationMap = new Dictionary<State, AnimationData>
                {
                    { State.Idle, AnimationHelper.QuickAnimData("EnemyKnight.Idle.png", 0.01f, false) },
                    { State.Run, AnimationHelper.QuickAnimData("EnemyKnight.Run.png", 1.25f, true) },
                    { State.Jump, AnimationHelper.QuickAnimData("EnemyKnight.Jump.png", 1.13f, false) }
                }
            }
        };

    public static Animation? MapStateToAnim(string CharType, State newState)
    {
        if (CharacterSettings.ContainsKey(CharType))
        {
            if (CharacterSettings[CharType].AnimationMap.ContainsKey(newState))
            {
                return AnimationHelper.QuickAnim(CharacterSettings[CharType].AnimationMap[newState]);
            }
            else if (CharacterSettings[CharType].AnimationMap.ContainsKey(State.Idle))
            {
                return AnimationHelper.QuickAnim(CharacterSettings[CharType].AnimationMap[State.Idle]);
            }
        }

        return null;
    }
}
