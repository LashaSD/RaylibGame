public class Signal<T>
{
    private readonly List<Action<T>> Callbacks = new();

    public Delegate? Connect(Action<T> callbackFn)
    {
        if (callbackFn != null)
        {
            this.Callbacks.Add(callbackFn);

            return () => {
                this.Callbacks.Remove(callbackFn);
            };
        }

        return null;
    }

    public void Fire(T args)
    {
        foreach (Action<T> callbackFn in this.Callbacks)
        {
            callbackFn(args);
        }
    }
}
