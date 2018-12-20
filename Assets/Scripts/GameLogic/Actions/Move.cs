using System;
using System.Collections;
using System.Collections.Generic;
using Hexlibrary;
using UnityEngine;

using GameLogic.Actions.Network;
using GameLogic.Units;

namespace GameLogic.Actions
{
    [Serializable]
    public class Move : GenericAction
    {
        public Move(GenericUnit genericUnit_)
            : base(genericUnit_)
        {
        }

        public override void EvaluatePossible(HexGrid hexGrid)
        {
            Hex currentPosition = hexGrid.units.getByType2(this.GenericUnit);
            int movementRange = this.GenericUnit.Movement;

            if (movementRange == 0)
            {
                this.IsPossible = false;
            }

            if (hexGrid.getHexesInMovementRange(currentPosition, movementRange).Length > 0)
            {
                this.IsPossible = true;
            }
            else
            {
                this.IsPossible = false;
            }
        }

        public override ActionType ActionType
        {
            get { return ActionType.SecondaryAction; }
        }

        public override ActionCategory ActionCategory
        {
            get { return ActionCategory.Move; }
        }

        public override GenericNetworkAction ExecuteAction(HexGrid hexGrid, Hex[] hexes)
        {
            return new MoveNetwork(hexGrid.units.getByType2(GenericUnit), hexes, ActionType);
        }

        public override List<Hex> PrepareAction(HexGrid hexGrid)
        {
            Hex currentPosition = hexGrid.units.getByType2(this.GenericUnit);
            int movementRange = this.GenericUnit.Movement;

            List<Hex> validHexes = new List<Hex>();

            validHexes.AddRange(hexGrid.getHexesInMovementRange(currentPosition, movementRange));

            hexGrid.SetCellsColour(validHexes.ToArray(), hexGrid.touchedColour);

            return validHexes;
        }
    }
}