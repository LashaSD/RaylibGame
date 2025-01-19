using FarseerPhysics.Collision;
using System.Numerics;

public class AttackAction1 : IAction
{
    public override State? PostExecutionState { get; protected set; } = State.Attack1;
    public override float? Duration { get; protected set; } = 0.55f;
    public override float? DebounceTime { get; protected set; } = 0.55f;
    public float Damage { get; protected set; } = 20;

    public override void Execute(Entity entity)
    {
        DynamicBodyComponent? dynamicBody = entity.GetComponent<DynamicBodyComponent>();
        TransformComponent? transform = entity.GetComponent<TransformComponent>();

        if (dynamicBody is null || dynamicBody.PhysicsBody is null || transform is null)
            return;
        
        Vector2 attackPosition = transform.Position;
        Vector2 attackRange = new(200, 200);
        AABB attackBox = new(
                PhysicsSystem.ToSimUnits(attackPosition - attackRange / 2),
                PhysicsSystem.ToSimUnits(attackPosition + attackRange / 2)
            );

        List<Entity> hitEntities = new List<Entity>();

        PhysicsSystem.PhysicsWorld.QueryAABB(fixture =>
            {
                if (fixture.Body.UserData is not Entity hitEntity)
                    return true;

                if (hitEntity.Id == entity.Id)
                    return true;

                hitEntities.Add(hitEntity);

                return true;
            }, 
            ref attackBox
        );

        // Apply damage to all hit entities
        foreach (Entity hitEntity in hitEntities)
        {
            var healthComponent = hitEntity.GetComponent<HealthComponent>();
            if (healthComponent != null)
            {
                Raylib.TraceLog(TraceLogLevel.Info, $"{entity.Id} {hitEntity.Id}");
                healthComponent.TakeDamage(this.Damage);
            }
        }

    }
}
