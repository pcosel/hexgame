using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using GameLogic;
using GameLogic.Actions;
using GameLogic.Actions.Network;
using Graphics;
using Hexlibrary;

namespace GameLogic.Units.Villains
{
    public partial class Sorcerer
    {
        internal class Precision : GenericAttack
        {
            internal Precision(Sorcerer sorcerer)
                : base(sorcerer, "Destruction", 0, 3, 100, 100, DamageType.Magic, 0, 1, 1, 1)
            {
            }

            public override ActionType ActionType
            {
                get { return ActionType.PrimaryAction; }
            }

            public override ActionCategory ActionCategory
            {
                get { return ActionCategory.Spell; }
            }

            public override void EvaluatePossible(HexGrid hexGrid)
            {
                Hex currentPosition = hexGrid.units.getByType2(this.GenericUnit);

                foreach (Hex possibleHex in hexGrid.getHexesInMovementRange(currentPosition, this.Range))
                {
                    GenericUnit unit = hexGrid.units.getByType1(possibleHex);
                    if (unit != null && unit.Faction.Equals(this.GenericUnit.Faction))
                    {
                        this.IsPossible = true;
                        return;
                    }
                }

                this.IsPossible = false;
            }

            private List<Hex> GetAllyHexes(HexGrid hexGrid)
            {
                Hex currentPosition = hexGrid.units.getByType2(this.GenericUnit);

                List<Hex> validHexes = new List<Hex>();
                Hex[] hexesInRange = hexGrid.getHexesInMovementRange(currentPosition, this.Range);

                foreach (var hex in hexesInRange)
                {
                    GenericUnit unit = hexGrid.units.getByType1(hex);
                    if (unit != null && unit.Faction.Equals(this.GenericUnit.Faction))
                    {
                        validHexes.Add(hex);
                    }
                }

                return validHexes;
            }

            public override List<Hex> PrepareAction(HexGrid hexGrid)
            {
                List<Hex> validHexes = GetAllyHexes(hexGrid);

                hexGrid.SetCellsColour(validHexes.ToArray(), hexGrid.touchedColour);

                return validHexes;
            }

            public override GenericNetworkAction ExecuteAction(HexGrid hexGrid, Hex[] hexes)
            {
                return new AttackNetwork(hexGrid.units.getByType2(GenericUnit), GetAllyHexes(hexGrid).ToArray(), this);
            }
        }

        internal class Aggression : GenericAttack
        {
            internal Aggression(Sorcerer sorcerer)
                : base(sorcerer, "Destruction", 0, 3, 100, 100, DamageType.Magic, 0, 1, 1, 1)
            {
            }

            public override ActionType ActionType
            {
                get { return ActionType.PrimaryAction; }
            }

            public override ActionCategory ActionCategory
            {
                get { return ActionCategory.Spell; }
            }

            public override void EvaluatePossible(HexGrid hexGrid)
            {
                Hex currentPosition = hexGrid.units.getByType2(this.GenericUnit);

                foreach (Hex possibleHex in hexGrid.getHexesInMovementRange(currentPosition, this.Range))
                {
                    GenericUnit unit = hexGrid.units.getByType1(possibleHex);
                    if (unit != null && unit.Faction.Equals(this.GenericUnit.Faction))
                    {
                        this.IsPossible = true;
                        return;
                    }
                }

                this.IsPossible = false;
            }

            private List<Hex> GetAllyHexes(HexGrid hexGrid)
            {
                Hex currentPosition = hexGrid.units.getByType2(this.GenericUnit);

                List<Hex> validHexes = new List<Hex>();
                Hex[] hexesInRange = hexGrid.getHexesInMovementRange(currentPosition, this.Range);

                foreach (var hex in hexesInRange)
                {
                    GenericUnit unit = hexGrid.units.getByType1(hex);
                    if (unit != null && unit.Faction.Equals(this.GenericUnit.Faction))
                    {
                        validHexes.Add(hex);
                    }
                }

                return validHexes;
            }

            public override List<Hex> PrepareAction(HexGrid hexGrid)
            {
                List<Hex> validHexes = GetAllyHexes(hexGrid);

                hexGrid.SetCellsColour(validHexes.ToArray(), hexGrid.touchedColour);

                return validHexes;
            }

            public override GenericNetworkAction ExecuteAction(HexGrid hexGrid, Hex[] hexes)
            {
                return new AttackNetwork(hexGrid.units.getByType2(GenericUnit), GetAllyHexes(hexGrid).ToArray(), this);
            }
        }
    }
}