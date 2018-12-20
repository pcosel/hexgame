using System;

using Hexlibrary;
using GameLogic.Actions;
using GameLogic.Actions.Network;
using GameLogic.Units;

[Serializable]
public class MoveNetwork : GenericNetworkAction
{

    public MoveNetwork(Hex unitHex_, Hex[] targetHexes_, ActionType actionType_)
        : base(unitHex_, targetHexes_, actionType_)
    {

    }

    public override void executeAction(HexGrid hexGrid)
    {
		base.executeAction (hexGrid);
        GenericUnit unit = hexGrid.units.getByType1(this.UnitHex);

        //remove unit from old field
        hexGrid.units.removeByType2(unit);
        //add to new field
        hexGrid.units.put(this.TargetHexes[0], unit);
    }
}
