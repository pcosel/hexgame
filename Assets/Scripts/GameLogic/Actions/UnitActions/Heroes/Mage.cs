using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using GameLogic;
using GameLogic.Actions;
using GameLogic.Actions.Network;
using Graphics;
using Hexlibrary;

namespace GameLogic.Units.Heroes
{
    public partial class Mage
    {
        internal class Destruction : GenericAttack
        {
            internal Destruction(Mage mage)
                : base(mage, "Destruction", 15, 5, 100, 100, DamageType.Magic, 0, 1, 1, 2)
            {
            }

            internal Destruction(Mage mage, int cooldown)
                : base(mage, "Destruction", 15, 5, 100, 100, DamageType.Magic, 0, 1, 1, cooldown)
            {
            }

            public override void EvaluatePossible(HexGrid hexGrid)
            {
                //TODO find cases where this is not possible
                IsPossible = true;
            }

            public override ActionType ActionType
            {
                get { return ActionType.PrimaryAction; }
            }

            public override ActionCategory ActionCategory
            {
                get { return ActionCategory.Spell; }
            }

            public override GenericNetworkAction ExecuteAction(HexGrid hexGrid, Hex[] hexes)
            {
                return new AttackNetwork(hexGrid.units.getByType2(GenericUnit), hexes, this);
            }
        }

        internal class DestructionAOE : Destruction
        {
            internal DestructionAOE(Mage mage)
                : base(mage, 3)
            {
            }

            public override void EvaluatePossible(HexGrid hexGrid)
            {
                //TODO find cases where this is not possible
                IsPossible = true;
            }

            public override GenericNetworkAction ExecuteAction(HexGrid hexGrid, Hex[] hexes)
            {
                //TODO apply AOE effect using network
                return new AttackNetwork(hexGrid.units.getByType2(GenericUnit), hexes, this);
            }
        }

        internal class Control : GenericAction
        {
            internal Control(Mage mage)
                : base(mage)
            {
            }

            public override void EvaluatePossible(HexGrid hexGrid)
            {
                //TODO find cases where this is not possible
                IsPossible = true;
            }

            public override ActionType ActionType
            {
                get { return ActionType.PrimaryAction; }
            }

            public override ActionCategory ActionCategory
            {
                get { return ActionCategory.Spell; }
            }

            public override List<Hex> PrepareAction(HexGrid hexGrid)
            {
                Hex currentPosition = hexGrid.units.getByType2(this.GenericUnit);
                int castRange = this.GenericUnit.Range;

                List<Hex> validHexes = new List<Hex>();

                validHexes.AddRange(hexGrid.getHexesInMovementRange(currentPosition, castRange));

                hexGrid.SetCellsColour(validHexes.ToArray(), hexGrid.touchedColour);

                return validHexes;
            }

            public override GenericNetworkAction ExecuteAction(HexGrid hexGrid, Hex[] hexes)
            {
                return new SpawnUnitNetwork(hexGrid.units.getByType2(GenericUnit), hexes, new ControlWall(), ActionType.PrimaryAction);
            }
        }
    }
}

