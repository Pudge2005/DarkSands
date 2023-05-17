using DevourDev.Ai;

namespace DevourDev.Utility
{
    public interface IAutoPoolableItem
    {
        void OnBeforeRent();

        void OnReturn();

        void OnPoolableItemDestroy();
    }
    public class ThreadSafeAutoPool<T> : ThreadSafePool<T>
        where T : class, IAutoPoolableItem, new()
    {
        public ThreadSafeAutoPool() : base()
        {
        }

        public ThreadSafeAutoPool(int bufferSize) : base(bufferSize)
        {
        }


        protected sealed override T CreateItem()
        {
            return new();
        }

        protected sealed override void DestroyItem(T item)
        {
            item.OnPoolableItemDestroy();
        }

        protected sealed override void OnBeforeItemRented(T item)
        {
            item.OnBeforeRent();
        }

        protected sealed override void OnItemReturned(T returnedItem, bool willBeDestroyed)
        {
            returnedItem.OnReturn();
        }
    }
}
