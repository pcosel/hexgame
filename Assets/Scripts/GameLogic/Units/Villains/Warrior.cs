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
    public partial class WarriorMelee : GenericUnit
    {
        public bool IsNextToAlly { get; private set; }

        public WarriorMelee(string name_)
            : base(name_, 50, 200, 3, 1, UnitSprite.ERROR, Roles.VILLAIN)
        {
            this.Faction = Faction.Villain;
            this.Actions.AddRange(
                new List<GenericAction>
                {
                    new BasicAttack(this, "Normal Attack", 10, this.Range, 80, 50, DamageType.Physical, 0, 1, 1, 1)
                }
            );
        }

        //TODO: find way to add conditional passive
    }

    [Serializable]
    public partial class WarriorArcher : GenericUnit
    {
        public int TimeSinceLastMove { get; private set; }

        public WarriorArcher(string name_)
            : base(name_, 30, 120, 3, 1, UnitSprite.ERROR, Roles.VILLAIN)
        {
            this.Faction = Faction.Villain;
            this.TimeSinceLastMove = 0;

            this.Actions.AddRange(
                new List<GenericAction>
                {
                    new BasicAttack(this, "Normal Attack", 10, this.Range, 100, 40, DamageType.Physical, 0, 1, 1, 1)
                }
            );
        }

        //TODO: find way to add conditional passive
    }
}
