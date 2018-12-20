using System;

namespace GameLogic.Units
{
    [Serializable]
    public abstract class GenericEffect
    {
        public readonly int EffectValue;
        public readonly int OverTimeValue;

        public int Duration { get; private set; }

        public readonly int ApplicationInterval;

        protected GenericEffect(int effectValue, int overTimeValue, int duration, int applicationInterval)
        {
            this.EffectValue = effectValue;
            this.OverTimeValue = overTimeValue;
            this.Duration = duration;
            this.ApplicationInterval = applicationInterval;
        }

        /// <summary>
        /// Class-specific application before duration decrement.
        /// </summary>
        /// <param name="genericUnit">Affected GenericUnit.</param>
        protected abstract void Apply(ref GenericUnit genericUnit);

        /// <summary>
        /// Apply the effect to a specified Attribute and decrement the duration.
        /// </summary>
        /// <param name="genericUnit">Affected unit.</param>
        public void ApplyEffect(ref GenericUnit genericUnit)
        {
            Apply(ref genericUnit);
            this.Duration--;
        }
    }
}

