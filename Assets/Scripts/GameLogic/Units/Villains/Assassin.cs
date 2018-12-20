using System;
using GameLogic;
using Graphics;

namespace GameLogic.Units.Villains
{
    [Serializable]
    public partial class Assassin : GenericUnit
    {

        public Assassin(string name_)
            : base(name_, 15, 30, 4, 1, UnitSprite.ASSASSIN, Roles.VILLAIN)
        {
        }
    }
}

