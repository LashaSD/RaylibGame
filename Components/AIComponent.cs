public class AIComponent : Component
{
    private MovementComponent? MovementComponent;
    private StateComponent? StateComponent;
    private ActionComponent? ActionComponent;

    private IAction Jump = new JumpAction();

    public AIComponent()
    {
        AISystem.Register(this);
    }

    public override void Init()
    {
        this.MovementComponent = this.ParentEntity?.GetComponent<MovementComponent>();
        this.StateComponent = this.ParentEntity?.GetComponent<StateComponent>();
        this.ActionComponent = this.ParentEntity?.GetComponent<ActionComponent>();
    }

    public override void Destroy()
    {
        AISystem.Remove(this);
    }

    public override void Update(float deltaTime)
    {
        if (this.MovementComponent is null || this.StateComponent is null ||  this.ActionComponent is null)
            return;

        this.ActionComponent.Execute(this.Jump);
    }
}
