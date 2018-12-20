using Hexlibrary;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using GameLogic.Actions.Network;
using GameLogic.Units;


namespace GameLogic.Actions
{
    public abstract class GenericAttack : GenericAction
    {
        public readonly string Name;
        public readonly int Damage;
        public readonly int Range;
        public readonly int Chance;
        public readonly DamageType DamageType;

        public readonly int DOTDamage;
        public readonly int Duration;
        public readonly int ApplicationInterval;
        public readonly int Cooldown;

        public int ArmourPenetration { get; private set; }

        protected GenericAttack(GenericUnit genericUnit_)
            : base(genericUnit_)
        {
        }

        protected GenericAttack(GenericUnit genericUnit_, string name, int damage, int range, int chance, int armorPenetration, DamageType damageType, int dotDamage, int duration, int applicationInterval, int cooldown)
            : base(genericUnit_)
        {
            this.Name = name;
            this.Damage = damage;
            this.Range = range;
            this.Chance = chance;
            this.ArmourPenetration = armorPenetration;
            this.DamageType = damageType;

            this.DOTDamage = dotDamage;
            this.Duration = duration;
            this.ApplicationInterval = applicationInterval;
            this.Cooldown = cooldown;
        }

        public override ActionType ActionType
        { 
            get { return ActionType.PrimaryAction; }
        }

        /// <summary>
        /// Indicates the number of targets an attack can affect.
        /// </summary>
        /// <value>The type of the attack target.</value>
        public AttackTargetType AttackTargetType
        { 
            get { return AttackTargetType.SingleTarget; }
        }

        public override ActionCategory ActionCategory
        {
            get { return ActionCategory.Attack; }
        }

        public override void EvaluatePossible(HexGrid hexGrid)
        {
            Hex currentPosition = hexGrid.units.getByType2(this.GenericUnit);
            int attackRange = this.GenericUnit.Range;

            if (attackRange == 0)
            {
                this.IsPossible = false;
            }

            foreach (Hex possibleHex in hexGrid.getHexesInMovementRange(currentPosition, attackRange))
            {
                if (hexGrid.units.getByType1(possibleHex) != null)
                {
                    this.IsPossible = true;
                    return;
                }
            }

            this.IsPossible = false;
        }

        public override List<Hex> PrepareAction(HexGrid hexGrid)
        {
            Hex currentPosition = hexGrid.units.getByType2(this.GenericUnit);
            int attackRange = this.GenericUnit.Range;

            List<Hex> validHexes = new List<Hex>();

            validHexes.AddRange(hexGrid.getHexesInMovementRange(currentPosition, attackRange));

            hexGrid.SetCellsColour(validHexes.ToArray(), hexGrid.touchedColour);

            return validHexes;
        }

        /// <summary>
        /// Calculates the potential damage to be shown on hover in the UI.
        /// </summary>
        /// <returns>The damage.</returns>
        /// <param name="target">The Target.</param>
        public int CalculateDamage(GenericUnit target)
        {
            if (DamageType.Equals(DamageType.Magic) || DamageType.Equals(DamageType.Poison))
            {
                return this.Damage;
            }
            else
            {
                // calculate penetration-based damage
                int penetrationdamage = (this.Damage * (this.ArmourPenetration / 100));
                int armourDamage = (this.Damage * (1 - this.ArmourPenetration / 100));

                // deduct remaining damage from target armour
                int penetrationBonus;

                if (armourDamage > target.Current_AP)
                {
                    //target.Current_AP = 0;
                    penetrationBonus = armourDamage - target.Current_AP;
                    return penetrationdamage + penetrationBonus;
                }
                else
                {
                    //target.Current_AP -= armourDamage;
                    return penetrationdamage;
                }
            }
        }

        public override GenericNetworkAction ExecuteAction(HexGrid hexGrid, Hex[] hexes)
        {
            return new AttackNetwork(hexGrid.units.getByType2(GenericUnit), hexes, this);
        }
    }
}