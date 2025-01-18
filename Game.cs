using Raylib_cs;
using System.Numerics;

public class Game 
{
    const int WindowWidth = 1600;
    const int WindowHeight = 900;

    public static void Main(string[] args)
    {
        Raylib.InitWindow(WindowWidth, WindowHeight, "Game");
        Raylib.SetTargetFPS(60);

        TextureManager.LoadTextures();

        GameWorld world = WorldReader.ReadFile();
        world.Construct();

        Entity player = EntityTemplates.PlayerEntity(new Vector2(100, WindowHeight - 250), new Vector2(80.0f, 86.0f));

        while (!Raylib.WindowShouldClose())
        {
            // Input & Other Systems
            InputSystem.Update(Raylib.GetFrameTime());
            ActionSystem.Update(Raylib.GetFrameTime());
            MovementSystem.Update(Raylib.GetFrameTime());
            AnimationSystem.Update(Raylib.GetFrameTime());
            // Rendering
            Raylib.BeginDrawing();
                Raylib.ClearBackground(Color.Black);

                RenderSystem.Update(Raylib.GetFrameTime());

                Raylib.DrawFPS(8, WindowHeight - 16 - 8);
            Raylib.EndDrawing();
            // Physics
            PhysicsSystem.Update(Raylib.GetFrameTime());
            DynamicBodySystem.Update(Raylib.GetFrameTime());
        }

        Raylib.CloseWindow();
    }
}
