using Raylib_cs;
using Microsoft.Xna.Framework;

class Entry 
{
    const int WindowWidth = 1600;
    const int WindowHeight = 900;

    public static void Main(string[] args)
    {
        Raylib.InitWindow(WindowWidth, WindowHeight, "Game");
        Raylib.SetTargetFPS(60);

        TextureManager.LoadTextures();

        Entity player = EntityTemplates.PlayerEntity(new Vector2(100, WindowHeight - 250), new Vector2(80, 86));
        {
            StateComponent? playerState = player.GetComponent<StateComponent>();
            playerState?.SetState(State.Run);
        }

        Entity floor = EntityTemplates.FloorEntity(new Vector2(0, WindowHeight - 100), new Vector2(WindowWidth, 100));
        {
            Texture2D? terrainTexture = TextureManager.TryGetTexture("TerrainTexture.png");
            if (terrainTexture is null)
                throw new Exception("Failed to fetch terrain textures");

            Rectangle sourceRect = TextureManager.GetTerrainTile(0, 0);

            RenderComponent? renderComponent = floor.GetComponent<RenderComponent>();
            renderComponent?.SetTexture((Texture2D) terrainTexture);
            renderComponent?.SetSourceRect(sourceRect);
        }

        while (!Raylib.WindowShouldClose())
        {
            // Input & Other Systems
            InputSystem.Update(Raylib.GetFrameTime());
            ActionSystem.Update(Raylib.GetFrameTime());
            AnimationSystem.Update(Raylib.GetFrameTime());
            // Rendering
            Raylib.BeginDrawing();
                Raylib.ClearBackground(Color.White);
                Raylib.DrawFPS(8, WindowHeight - 16 - 8);

                RenderSystem.Update(Raylib.GetFrameTime());
            Raylib.EndDrawing();
            // Physics
            PhysicsSystem.Update(Raylib.GetFrameTime());
        }

        Raylib.CloseWindow();
    }
}
