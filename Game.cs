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

        TextureManager textureManager = new();
        EntityManager entityManager = new();

        textureManager.LoadTextures();

        Texture2D? knightTexture = textureManager.TryGetTexture("Run.png");
        Entity e = EntityTemplates.PlayerEntity((Texture2D) knightTexture);
        entityManager.AddEntity(e);

        RenderSystem renderSystem = new RenderSystem(entityManager);
        AnimationSystem animationSystem = new AnimationSystem(entityManager, textureManager);

        Array states = Enum.GetValues(typeof(State));
        float lastTime = (float) Raylib.GetTime();

        while (!Raylib.WindowShouldClose())
        {
            // Input
            if (Raylib.IsKeyDown(KeyboardKey.Space) && Raylib.GetTime() - lastTime >= 0.15f)
            {
                StateComponent stateComponent = e.TryGetComponent<StateComponent>();

                int state = (int) (stateComponent.CurrentState);
                int limit = TextureTiles.Textures["Attack3.png"];

                animationSystem.frameIndex = (animationSystem.frameIndex + 1) % limit;

                lastTime = (float) Raylib.GetTime();
            }

            animationSystem.Update(Raylib.GetFrameTime());

            // Rendering

            Raylib.BeginDrawing();
                Raylib.ClearBackground(Color.Black);
                renderSystem.Render();
            Raylib.EndDrawing();
            // Physics
        }

        Raylib.CloseWindow();
    }
}
