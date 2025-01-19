public class HealthComponent : Component
{
    public float Health { get; private set; } = 100.0f;
    public bool DeathOnZero { get; private set; } = true;

    public HealthComponent()
    { }

    public HealthComponent(float StartingHealth)
    {
        this.Health = StartingHealth;
    }

    public void UpdateHealth()
    {
        if (this.Health <= 0 && this.DeathOnZero)
            this.ParentEntity?.Destroy();
    }

    public void TakeDamage(float dmg)
    {
        this.Health -= dmg;
        this.UpdateHealth();
    }
}
