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

        EntityDataContainer entityData = WorldReader.ReadFile(Path.Join("Config", "World.json"));
        GameWorld World = new();
        World.Construct(entityData);

        while (!Raylib.WindowShouldClose())
        {
            // Input & Other Systems
            InputSystem.Update(Raylib.GetFrameTime());
            AISystem.Update(Raylib.GetFrameTime());
            ActionSystem.Update(Raylib.GetFrameTime());
            AnimationSystem.Update(Raylib.GetFrameTime());
            CameraSystem.Update(Raylib.GetFrameTime());
            // Rendering
            Raylib.BeginDrawing();
                Raylib.ClearBackground(Color.Black);

                CameraSystem.Begin();
                    RenderSystem.Update(Raylib.GetFrameTime());
                CameraSystem.End();

                Raylib.DrawFPS(8, WindowHeight - 16 - 8);
            Raylib.EndDrawing();
            // Physics
            PhysicsSystem.Update(Raylib.GetFrameTime());
            DynamicBodySystem.Update(Raylib.GetFrameTime());

            //Heartbeat
            World.Heartbeat();
        }

        Raylib.CloseWindow();
    }
}
