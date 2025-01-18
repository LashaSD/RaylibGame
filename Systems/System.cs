class BaseSystem<T> where T : Component
{
    protected static List<T> components = new List<T>();

    public static void Register(T component)
    {
        components.Add(component);
    }

    public static void Update(float deltaTime)
    {
        foreach (T component in components)
        {
            component.Update(deltaTime);
        }
    }
}

class TransformSystem : BaseSystem<TransformComponent> { }
class RenderSystem : BaseSystem<RenderComponent> { }
class AnimationSystem : BaseSystem<AnimationComponent> { }
class StateSystem : BaseSystem<StateComponent> { }
class InputSystem : BaseSystem<InputComponent> { }
class ActionSystem : BaseSystem<ActionComponent> { }
class DynamicBodySystem : BaseSystem<DynamicBodyComponent> { }
