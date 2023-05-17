namespace DevourDev.Unity.CharacterControlling.Movement
{
    public interface IMovementStats
    {
        public delegate void MoveSpeedChangedDelegate(IMovementStats stats, float prevSpeed, float newSpeed);


        float MoveSpeed { get; }


        event MoveSpeedChangedDelegate MoveSpeedChanged;
    }
}
