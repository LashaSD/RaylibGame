using Raylib_cs;

class CameraSystem : BaseSystem<CameraComponent> 
{ 
    public static void Begin()
    {
        if(components.Count == 0)
            return;

        Raylib.BeginMode2D(components.First().Camera);
    }

    public static void End()
    {
        if (components.Count == 0)
            return;

        Raylib.EndMode2D();
    }
}
