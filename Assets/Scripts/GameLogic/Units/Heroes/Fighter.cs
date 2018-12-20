using System;
using System.Collections.Generic;

using GameLogic.Actions;
using GameLogic.Actions.Network;
using Graphics;
using Hexlibrary;

namespace GameLogic.Units.Heroes
{
    [Serializable]
    public partial class Fighter : GenericUnit
    {
        public Stance Stance;

        public Fighter(string name_)
            : base(name_, 100, 400, 3, 1, UnitSprite.FIGHTER, Roles.FIGHTER)
        {
            this.Faction = Faction.Hero;

            this.Actions.AddRange(
                new List<GenericAction>
                {
                    new BasicAttack(this, "Normal Attack", 15, 2, 75, 50, DamageType.Physical, 0, 1, 1, 1),
                    new BasicAttack(this, "Power attack", 20, 2, 50, 25, DamageType.Physical, 0, 1, 1, 1),
                    new BasicAttack(this, "Precise attack", 10, 2, 95, 75, DamageType.Physical, 0, 1, 1, 1),
                    new ChangeStance(this),
                    new PushBack(this)
                }
            );
        }

        private float CalculateStanceMitigationFactor(DamageType attacktype)
        {
            return 1 - this.Stance.GetDamageMitigation(attacktype) / 100;
        }
    }
}

