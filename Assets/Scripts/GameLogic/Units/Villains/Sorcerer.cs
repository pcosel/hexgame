using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using GameLogic;
using GameLogic.Actions;
using Graphics;

namespace GameLogic.Units.Villains
{
    [Serializable]
    public partial class Sorcerer : GenericUnit
    {
        public Sorcerer(string name_, int max_HP_, int max_AP_, int movement_)
            : base(name_, 40, 80, 4, 1, UnitSprite.ERROR, Roles.VILLAIN)
        {
            this.Faction = Faction.Villain;
            this.Actions.AddRange(
                new List<GenericAction>
                {
                    new BasicAttack(this, "Normal Attack", 6, this.Range, 75, 50, DamageType.Physical, 0, 1, 1, 1),
                    new Precision(this),
                    new Aggression(this)
                }
            );

        }
    }
}
