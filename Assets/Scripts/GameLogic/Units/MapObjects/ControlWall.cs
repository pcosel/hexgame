using System;

namespace GameLogic.Units
{
    public class ControlWall : GenericUnit
    {
        public ControlWall()
            : base("Control wall", 0, 7, 0, 0, Graphics.UnitSprite.ERROR, Roles.MAGE)
        {
        }
    }
}

