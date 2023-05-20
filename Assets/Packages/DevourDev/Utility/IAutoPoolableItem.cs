namespace DevourDev.Utility
{
    public interface IAutoPoolableItem
    {
        void OnBeforeRent();

        void OnReturn();

        void OnPoolableItemDestroy();
    }
}
