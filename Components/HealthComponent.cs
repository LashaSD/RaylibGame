public class HealthComponent : Component
{
    public float Health { get; private set; } = 100.0f;
    public bool DeathOnZero { get; private set; } = true;

    private StateComponent? StateComponent;
    private AnimationComponent? AnimationComponent;

    public HealthComponent()
    { }

    public HealthComponent(float StartingHealth)
    {
        this.Health = StartingHealth;
    }

    public override void Init()
    {
        this.StateComponent = this.ParentEntity?.GetComponent<StateComponent>();
    }

    public void UpdateHealth()
    {
        if (this.Health <= 0 && this.DeathOnZero)
        {
            this.ParentEntity?.MarkDestroy();
            this.StateComponent?.SetState(State.Dead);
        }
    }

    public void TakeDamage(float dmg)
    {
        this.Health -= dmg;
        this.UpdateHealth();
        this.StateComponent?.SetState(State.Hurt);
    }
}
