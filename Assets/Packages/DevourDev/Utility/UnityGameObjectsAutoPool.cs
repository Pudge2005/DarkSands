namespace DevourDev.Utility
{
    public class UnityGameObjectsAutoPool<T> : UnityAutoPool<T>
        where T : UnityEngine.Component, IAutoPoolableItem
    {
        protected override void OnBeforeItemRented(T item)
        {
            base.OnBeforeItemRented(item);
            item.gameObject.SetActive(true);
        }

        protected override void OnItemReturned(T returnedItem, bool willBeDestroyed)
        {
            base.OnItemReturned(returnedItem, willBeDestroyed);

            if (!willBeDestroyed)
            {
                returnedItem.gameObject.SetActive(false);
            }
        }

        protected override void DestroyItem(T item)
        {
            base.DestroyItem(item);
            Destroy(item.gameObject);
        }
    }
}
