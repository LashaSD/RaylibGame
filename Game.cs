using Raylib_cs;
using System.Numerics;

class Entry 
{
    const int WindowWidth = 800;
    const int WindowHeight = 600;

    public static void Main(string[] args)
    {
        Raylib.InitWindow(WindowWidth, WindowHeight, "Game");
        Raylib.SetTargetFPS(60);

        TextureManager.LoadTextures();

        Entity player = EntityTemplates.PlayerEntity();
        {
            TransformComponent? transform = player.GetComponent<TransformComponent>();
            transform?.SetScale(new Vector2(4, 4));
            player[TransformComponent]?.SetScale();

            StateComponent? playerState = player.GetComponent<StateComponent>();
            playerState?.SetState(State.Run);
        }

        while (!Raylib.WindowShouldClose())
        {
            // Input
            AnimationSystem.Update(Raylib.GetFrameTime());
            // Rendering
            Raylib.BeginDrawing();
                Raylib.ClearBackground(Color.Black);
                Raylib.DrawFPS(8, WindowHeight - 16 - 8);

                RenderSystem.Update(Raylib.GetFrameTime());
            Raylib.EndDrawing();
            // Physics
        }

        Raylib.CloseWindow();
    }
}
