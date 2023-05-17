using DarkSands.Characters;
using DevourDev.Ai;

namespace DarkSands.Ai
{
    public sealed class AiContext : AiContextBase
    {
        public Character Character { get; set; }


        public override void OnBeforeRent()
        {
            base.OnBeforeRent();

        }

        public override void OnPoolableItemDestroy()
        {
            base.OnPoolableItemDestroy();

        }

        public override void OnReturn()
        {
            base.OnReturn();
            Character = null;
        }
    }
}
