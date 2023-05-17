using System.Collections.Generic;

namespace DevourDev.Utility
{
    public class ThreadSafeDictionariesPool<TKey, TValue> : ThreadSafePool<Dictionary<TKey, TValue>>
    {
        public ThreadSafeDictionariesPool() : this(DefaultBufferSize)
        {
        }

        public ThreadSafeDictionariesPool(int bufferSize) : base(bufferSize)
        {
        }

        protected override Dictionary<TKey, TValue> CreateItem()
        {
            return new();
        }

        protected override void OnItemReturned(Dictionary<TKey, TValue> returnedItem, bool willBeDestroyed)
        {
            returnedItem.Clear();
        }
    }
}
