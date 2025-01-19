using System.Numerics;

public static class EntityTemplates
{
    public static EntityData PlayerData = new() {
        Position = new Vector2(200, 100),
        Size = new Vector2(45, 86),
        TextureFile = "PlayerKnight.Idle.png",
        Type = EntityType.Player,
        Density = 142
    };
}
