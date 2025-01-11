using Raylib_cs;

class Entry 
{
    const int WindowWidth = 800;
    const int WindowHeight = 600;

    public static void Main(string[] args)
    {
        Raylib.InitWindow(WindowWidth, WindowHeight, "Game");
        Raylib.SetTargetFPS(60);

        EntityManager entityManager = new();
        Texture2D texture = Raylib.LoadTexture("./Assets/Knight/Hurt.png");
        Entity e = EntityTemplates.PlayerEntity(texture);
        entityManager.AddEntity(e);

        RenderSystem renderSystem = new RenderSystem(entityManager);
        
        while (!Raylib.WindowShouldClose())
        {
            // Input
            // Rendering
            Raylib.BeginDrawing();
                renderSystem.Render();
            Raylib.EndDrawing();
            // Physics
        }

        Raylib.CloseWindow();
    }
}
