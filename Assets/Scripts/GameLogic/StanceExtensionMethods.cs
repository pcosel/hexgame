using System;
using GameLogic.Actions;

namespace GameLogic.Units
{
    public static class StanceExtensionMethods
    {
        public static int GetDamageMitigation(this Stance stance, DamageType attackType)
        {
            switch (stance)
            {
                case Stance.DefendMagical:
                    return attackType.Equals(DamageType.Magic)
                        ? 70
                        : 20;
                case Stance.DefendPhysical:
                    return attackType.Equals(DamageType.Physical)
                        ? 70
                        : 20;
                default:
                    return attackType.Equals(DamageType.Poison)
                        ? 70
                        : 20;
            }
        }
    }
}

