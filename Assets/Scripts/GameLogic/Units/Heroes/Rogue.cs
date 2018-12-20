using System;
using System.Collections.Generic;
using UnityEngine;

using GameLogic.Actions;
using Graphics;

namespace GameLogic.Units.Heroes
{
    [Serializable]
    public partial class Rogue : GenericUnit
    {

        public Rogue(string name_)
            : base(name_, 100, 200, 4, 1, UnitSprite.ROGUE, Roles.ROGUE)
        {
            this.Faction = Faction.Hero;

            this.Actions.AddRange(
                new List<GenericAction>
                {
                    new BasicAttack(this, "Normal Attack", 15, this.Range, 50, 50, DamageType.Physical, 0, 1, 1, 1),
                    new BasicAttack(this, "Poisoned Dagger", 8, this.Range, 80, 100, DamageType.Poison, 2, 3, 1, 3),
                    new PoisonCloud(this),
                    new Tripwire(this),
                    new Lookout(this),
                    new Planeshift(this)
                }
            );
        }
    }
}
