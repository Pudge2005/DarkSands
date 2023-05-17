using System;

namespace DevourDev.Unity.Diplomacy
{
    [Flags]
    public enum AllyFlags : byte
    {
        None = 0b0,
        Any = byte.MaxValue,
        Ally = 0b1,
        Enemy = 0b10,
        Neutral = 0b100,
    }
}
