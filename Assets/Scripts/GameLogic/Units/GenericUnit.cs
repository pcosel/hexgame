using System;
using System.Collections.Generic;

using Hexlibrary;
using GameLogic;
using Graphics;
using GameLogic.Actions;
using GameLogic.Units;

namespace GameLogic.Units
{
    [Serializable]
    public abstract class GenericUnit
    {

        /// <summary>
        /// An enumeration referencing the sprite for this unit.
        /// </summary>
        public UnitSprite Sprite;

        public readonly Roles owner;

        public bool IsHero
        {
            get
            {
                return owner == Roles.FIGHTER || owner == Roles.ROGUE || owner == Roles.MAGE;
            }
        }

        public readonly string Name;

        public readonly int Max_HP;
        public readonly int Max_AP;
        public readonly int Movement;
        public readonly int Range;

        public int Current_HP { get; internal set; }

        public int Current_AP { get; internal set; }

        public Faction Faction { get; protected set; }

        public List<GenericEffect> Effects { get; internal set; }

        /// <summary>
        /// The actions this unit can use. Should be set with default values.
        /// </summary>
        public List<GenericAction> Actions { get; private set; }

        public Dictionary<ActionType, bool> UsedActionType = new Dictionary<ActionType, bool>()
        {
            { ActionType.PrimaryAction, false },
            { ActionType.SecondaryAction, false }
        };

        /// <summary>
        /// Creates a new unit. Throws an argument exception if one of the argument requirement is not met.
        /// </summary>
        /// <param name="name_">The name of the unit. (May not be null)</param>
        /// <param name="max_HP_">The Maximum HP of this unit. (Needs to be greater than zero)</param>
        /// <param name="max_AP_">The Maximum AP of this unit. (Needs to be greater than -1)</param>
        /// <param name="movement_">The Maximum number of hexes this unit can move with one action. (Needs to be greater than -1)</param>
        /// <param name="movement_">The Maximum distance this unit execute actions on. (Needs to be greater than -1)</param>
        /// <param name="sprite_">The sprite to represent this unit in the 2d view.</param>
        /// <param name="owner_">The player that controlls this unit.</param>
        internal GenericUnit(string name_, int max_HP_, int max_AP_, int movement_, int range_, UnitSprite sprite_, Roles owner_)
        {
            if (name_ == null)
            {
                throw new System.ArgumentException("Parameter cannot be null", "name_");
            }
            if (max_HP_ <= 0)
            {
                throw new System.ArgumentException("Parameter cannot be smaller than or equal to zero.", "max_HP_");
            }
            if (max_AP_ < 0)
            {
                throw new System.ArgumentException("Parameter cannot be smaller than zero.", "max_AP_");
            }
            if (movement_ < 0)
            {
                throw new System.ArgumentException("Parameter cannot be smaller than zero.", "movement_");
            }
            if (range_ < 0)
            {
                throw new System.ArgumentException("Parameter cannot be smaller than zero.", "range_");
            }

            Name = name_;
            Faction = Faction.Neutral;
            Current_HP = Max_HP = max_HP_;
            Current_AP = Max_AP = max_AP_;

            Movement = movement_;
            Range = range_;

            owner = owner_;
            Sprite = sprite_;

            this.Actions = new List<GenericAction>();

            this.Actions.AddRange(
                new List<GenericAction>
                {
                    new Move(this)
                }
            );

        }

        /// <summary>
        /// Adds an effect to the unit.
        /// </summary>
        /// <param name="effect">Effect.</param>
        public void AddEffect(GenericEffect effect)
        {
            Effects.Add(effect);
        }

        /// <summary>
        /// Applies the effect.
        /// </summary>
        /// <param name="effect">Effect.</param>
        public void ApplyEffect(GenericEffect effect)
        {
            //HACK Unsightly way to pass "this" as reference
            var currentObject = this;
            effect.ApplyEffect(ref currentObject);
        }

        /// <summary>
        /// Applies all listed effects.
        /// </summary>
        public void ApplyEffects()
        {
            foreach (var effect in Effects)
            {
                ApplyEffect(effect);
                if (effect.Duration < 2)
                {
                    Effects.Remove(effect);
                }
            }
        }

        /// <summary>
        /// Lets each action evalute if it could be executed under the current conditions and returns the checked actions.
        /// </summary>
        /// <returns>The actions with updated possible-field.</returns>
        /// <param name="hexGrid">The grid handler representing the current game state.</param>
        public List<GenericAction> GetActions(HexGrid hexGrid)
        {
            foreach (GenericAction genericAction in Actions)
            {
                genericAction.EvaluatePossible(hexGrid);
            }
            return Actions;
        }
    }

}