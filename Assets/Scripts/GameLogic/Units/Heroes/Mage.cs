using System;
using System.Collections.Generic;
using GameLogic;

using GameLogic.Actions;
using GameLogic.Actions.Network;
using Graphics;
using Hexlibrary;
using UnityEngine;

namespace GameLogic.Units.Heroes
{
    [Serializable]
    public partial class Mage : GenericUnit
    {
        public Mage(string name_)
            : base(name_, 100, 100, 3, 1, UnitSprite.MAGE, Roles.MAGE)
        {
            this.Faction = Faction.Hero;

            this.Actions.AddRange(
                new List<GenericAction>
                {
                    new BasicAttack(this, "Normal Attack", 15, this.Range, 50, 50, DamageType.Physical, 0, 1, 1, 1),
                    new Destruction(this),
                    new DestructionAOE(this),

                }
            );
        }
    }

}
