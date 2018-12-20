using System;

namespace GameLogic.Units
{
    public class EffectReduceAP : GenericEffect
    {
        public EffectReduceAP(int reduction)
            : base(reduction, 0, 1, 1)
        {
        }

        protected override void Apply(ref GenericUnit genericUnit)
        {
            genericUnit.Current_AP = this.EffectValue > genericUnit.Current_AP
                    ? 0
                : genericUnit.Current_AP - this.EffectValue;
        }
    }
}

