using System;

using Hexlibrary;
using GameLogic.Units;

namespace GameLogic.Actions.Network
{
    public class SpawnUnitNetwork : GenericNetworkAction
    {
        public readonly GenericUnit Spawn;

        public SpawnUnitNetwork(Hex unitHex_, Hex[] targetHexes_, GenericUnit genericUnit_, ActionType actionType_)
            : base(unitHex_, targetHexes_, actionType_)
        {
            Spawn = genericUnit_;
        }

        public override void executeAction(HexGrid hexGrid)
        {
			base.executeAction (hexGrid);
            foreach (var targetHex in TargetHexes)
            {
                hexGrid.units.put(targetHex, Spawn);
            }

        }
    }
}

