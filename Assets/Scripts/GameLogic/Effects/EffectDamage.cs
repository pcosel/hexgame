using System;

namespace GameLogic.Units
{
    public class EffectDamage : GenericEffect
    {

        public EffectDamage(int damageAmount, int dotDamage, int duration, int applicationInterval)
            : base(damageAmount, dotDamage, duration, applicationInterval)
        {
        }

        protected override void Apply(ref GenericUnit genericUnit)
        {
            genericUnit.Current_AP -= this.EffectValue;
        }
    }
}
