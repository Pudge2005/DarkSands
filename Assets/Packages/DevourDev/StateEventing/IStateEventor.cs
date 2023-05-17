namespace DevourDev.StateEventing
{
    public interface IStateEventor<T>
    {
        T State { get; }

        event StateChangedArgs<T> OnStateChanged;
    }
}
