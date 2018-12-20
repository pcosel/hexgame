using System;
using GameLogic.Units;

namespace GameLogic.Actions
{
    public class BasicAttack : GenericAttack
    {
        public BasicAttack(GenericUnit genericUnit_, string name, int damage, int range, int chance, int armorPenetration, DamageType attackType, int dotDamage, int duration, int applicationInterval, int cooldown)
            : base(genericUnit_, name, damage, range, chance, armorPenetration, attackType, dotDamage, duration, applicationInterval, cooldown)
        {
        }
    }
}

