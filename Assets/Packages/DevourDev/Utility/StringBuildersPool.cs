using System.Text;

namespace DevourDev.Utility
{
    public sealed class StringBuildersPool : ThreadSafePool<StringBuilder>
    {
        protected override StringBuilder CreateItem()
        {
            return new StringBuilder();
        }

        protected override void OnItemReturned(StringBuilder returnedItem, bool willBeDestroyed)
        {
            if (willBeDestroyed)
                return;

            returnedItem.Clear();
        }

    }
}
