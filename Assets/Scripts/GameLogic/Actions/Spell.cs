using System;
using System.Collections.Generic;
using Hexlibrary;

using GameLogic.Units;
using GameLogic.Actions.Network;

namespace GameLogic.Actions
{
    public class Spell : GenericAttack
    {
        public Spell(GenericUnit genericUnit_)
            : base(genericUnit_)
        {
        }

        public Spell(GenericUnit genericUnit_, string name, int damage, int range, int chance, int armorPenetration, DamageType damageType, int dotDamage, int duration, int applicationInterval, int cooldown)
            : base(genericUnit_, name, damage, range, chance, armorPenetration, damageType, dotDamage, duration, applicationInterval, cooldown)
        {
        }
    }
}