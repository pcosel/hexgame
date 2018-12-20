using System;
using Hexlibrary;

namespace GameLogic.Units
{
    public class EffectChangePosition : GenericEffect
    {
        public readonly Hex TargetPosition;

        public EffectChangePosition(Hex hex)
            : base(0, 0, 1, 1)
        {
            this.TargetPosition = hex;
        }

        protected override void Apply(ref GenericUnit genericUnit)
        {
            //TODO: Replace hexGrid entry for unit with new hex position
        }
    }
}

