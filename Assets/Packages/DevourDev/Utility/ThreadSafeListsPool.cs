using System.Collections.Generic;

namespace DevourDev.Utility
{
    public class ThreadSafeListsPool<T> : ThreadSafePool<List<T>>
    {
        public ThreadSafeListsPool() : this(DefaultBufferSize)
        {

        }

        public ThreadSafeListsPool(int bufferSize) : base(bufferSize)
        {
        }


        protected override List<T> CreateItem()
        {
            return new();
        }

        protected override void OnItemReturned(List<T> returnedItem, bool willBeDestroyed)
        {
            returnedItem.Clear();
        }
    }
}
