using System.Numerics;
using FarseerPhysics.Collision;
using Raylib_cs;

public static class AttackHelper
{
    public static HashSet<Entity> QueryAttack(int attackerId, Vector2 atkPos, Vector2 atkRange)
    {
        AABB attackBox = new(
                PhysicsSystem.ToSimUnits(atkPos - atkRange / 2),
                PhysicsSystem.ToSimUnits(atkPos + atkRange / 2)
            );

        HashSet<Entity> hitEntities = new();

        PhysicsSystem.PhysicsWorld.QueryAABB(fixture =>
            {
                if (fixture.Body.UserData is not Entity hitEntity)
                    return true;

                if (hitEntity.Id == attackerId)
                    return true;

                hitEntities.Add(hitEntity);

                return true;
            }, 
            ref attackBox
        );

        return hitEntities;
    }

    public static void GeneralAttack(Entity Attacker, Vector2 AtkRange, float Dmg, float Pushback)
    {
        DynamicBodyComponent? selfDynamicBody = Attacker.GetComponent<DynamicBodyComponent>();
        TransformComponent? selfTransform = Attacker.GetComponent<TransformComponent>();

        if (selfDynamicBody is null || selfDynamicBody.PhysicsBody is null || selfTransform is null)
            return;
        
        Vector2 offset = new Vector2(selfTransform.FaceDirection.X  * 1, 0);
        Vector2 attackPosition = selfTransform.Position + offset;
        Vector2 attackRange = new(AtkRange.X, AtkRange.Y);

        HashSet<Entity> hitEntities = AttackHelper.QueryAttack(Attacker.Id, attackPosition, attackRange);

        foreach (Entity hitEntity in hitEntities)
        {
            HealthComponent? healthComponent = hitEntity.GetComponent<HealthComponent>();
            DynamicBodyComponent? dynamicBody = hitEntity.GetComponent<DynamicBodyComponent>();
            StateComponent? state = hitEntity.GetComponent<StateComponent>();
            ActionComponent? action = hitEntity.GetComponent<ActionComponent>();

            if (healthComponent is not null)
            {
                if (state is not null && state.CurrentState == State.Defend)
                {
                    if (action is not null)
                    {
                        action.ResetAction();
                        action.ActionTimer["DefendAction"] = (float) Raylib.GetTime();
                    }
                } else 
                {
                    healthComponent.TakeDamage(Dmg);
                }
            }

            if (dynamicBody is not null)
            {
                Vector2 impulse = new Vector2(Pushback * selfTransform.FaceDirection.X, 0);
                dynamicBody.PhysicsBody?.ApplyLinearImpulse(PhysicsSystem.ToSimUnits(impulse));
            }
        }
    }
}
