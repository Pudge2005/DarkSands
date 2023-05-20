using System;

namespace DevourDev.Utility
{
    public class ThreadSafeGenericAutoPool<T> : ThreadSafePool<T>
        where T : class, IAutoPoolableItem
    {
        private readonly Func<T> _createItemFunc;
        private readonly Action<T> _destroyItemAction;


        public ThreadSafeGenericAutoPool(Func<T> createItemFunc,
            Action<T> destroyItemAction) : base()
        {
            _createItemFunc = createItemFunc ?? throw new ArgumentNullException(nameof(createItemFunc));
            _destroyItemAction = destroyItemAction;
        }

        public ThreadSafeGenericAutoPool(int bufferSize, Func<T> createItemFunc,
            Action<T> destroyItemAction) : base(bufferSize)
        {
            _createItemFunc = createItemFunc ?? throw new ArgumentNullException(nameof(createItemFunc));
            _destroyItemAction = destroyItemAction;
        }


        protected sealed override T CreateItem()
        {
            return _createItemFunc();
        }

        protected sealed override void DestroyItem(T item)
        {
            if (item != null)
            {
                item.OnPoolableItemDestroy();
                _destroyItemAction?.Invoke(item);
            }
        }

        protected sealed override void OnBeforeItemRented(T item)
        {
            item.OnBeforeRent();
        }

        protected sealed override void OnItemReturned(T returnedItem, bool willBeDestroyed)
        {
            returnedItem?.OnReturn();
        }
    }
}
