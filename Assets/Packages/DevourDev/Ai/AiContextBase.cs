using DevourDev.Utility;

namespace DevourDev.Ai
{
    public abstract class AiContextBase : IAutoPoolableItem
    {
        public float DeltaTime { get; set; }
        public Fsm StateMachine { get; internal set; }


        public virtual void OnBeforeRent()
        {
        }

        public virtual void OnPoolableItemDestroy()
        {

        }

        public virtual void OnReturn()
        {
            DeltaTime = default;
            StateMachine = null;
        }
    }
}
