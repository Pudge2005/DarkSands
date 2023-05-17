namespace DevourDev.Stats
{
    public interface IClampedStatData<TKey, TValue> : IStatData<TKey, TValue>
    {
        TValue Min { get; }
        TValue Max { get; }
    }
}
