using System;

using Hexlibrary;
using GameLogic.Units;
using GameLogic.Units.Heroes;

namespace GameLogic.Actions.Network
{
    [Serializable]
    public class ChangeStanceNetwork : GenericNetworkAction
    {

        Stance stance;

        public ChangeStanceNetwork(Stance stance_, Hex unitHex_, Hex[] targetHexes_, ActionType actionType_)
            : base(unitHex_, targetHexes_, actionType_)
        {
            stance = stance_;
        }

        public override void executeAction(HexGrid hexGrid)
        {
			base.executeAction (hexGrid);
            Fighter fighter = (Fighter)hexGrid.units.getByType1(UnitHex);
            fighter.Stance = stance;
        }
    }
}