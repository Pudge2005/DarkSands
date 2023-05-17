namespace DevourDev.Ai
{
    public abstract class AiSensorDataBase
    {
        private bool _isRelevant;


        public AiSensorDataBase()
        {
            _isRelevant = false;
        }


        public bool IsRelevant
        {
            get => _isRelevant;
            internal set => _isRelevant = value;
        }

    }
}
