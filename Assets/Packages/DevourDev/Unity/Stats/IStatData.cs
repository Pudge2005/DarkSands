namespace DevourDev.Stats
{
    public interface IStatData<TKey, TValue>
    {
        TKey Stat { get; }
        TValue Value { get; }
    }
}
