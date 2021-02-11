public abstract class State<T>
{
    protected string stateName;
    public virtual State<T> createInstance() { return null; }
    public virtual string GetStateName() => stateName;
    public abstract void EnterState(T owner);
    public abstract void ExitState(T owner);
    public abstract void UpdateState(T owner);
    public abstract void FixedUpdateState(T owner);
}
