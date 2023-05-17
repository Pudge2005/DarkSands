namespace DevourDev.Unity.TwoDThreeD
{
    public interface IAnimator
    {
        void SetFloat(int hash, float value);
        void SetInt(int hash, int value);
        void SetBool(int hash, bool value);
        void SetTrigger(int hash);

        float GetFloat(int hash);
        int GetInt(int hash);
        bool GetBool(int hash);
    }
}
