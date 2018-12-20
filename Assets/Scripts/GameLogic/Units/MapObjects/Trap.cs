using System;

using GameLogic.Actions;

namespace GameLogic.Units
{
    public class Trap : GenericUnit
    {
        GenericAttack TriggerAttack;

        public Trap(GenericAttack genericAttack)
            : base(genericAttack.Name, 0, 0, 0, genericAttack.Range, Graphics.UnitSprite.ERROR, Roles.ROGUE)
        {
            this.TriggerAttack = genericAttack;
        }

        //TODO trigger attack
    }
}

