using System;

using UnityEngine;

using Hexlibrary;
using GameLogic.Units;

namespace GameLogic.Actions.Network
{

    [Serializable]
    public abstract class GenericNetworkAction
    {

        protected Hex UnitHex;

        protected Hex[] TargetHexes;

        protected ActionType ActionType;

        protected GenericNetworkAction(Hex unitHex_, Hex[] targetHexes_, ActionType actionType_)
        {
            UnitHex = unitHex_;
            TargetHexes = targetHexes_;
            ActionType = actionType_;
        }

		public virtual void executeAction(HexGrid hexGrid) {
			GenericUnit unit = hexGrid.battlefield.units.getByType1 (UnitHex);

			if (ActionType == ActionType.SecondaryAction && unit.UsedActionType[ActionType.SecondaryAction]) {
				if (unit.UsedActionType [ActionType.PrimaryAction])
					Debug.Log ("WARNING: trying to use secondary action but no action available.");
				unit.UsedActionType [ActionType.PrimaryAction] = true;
			} else {
				unit.UsedActionType [ActionType] = true;
			}
		}

    }
}