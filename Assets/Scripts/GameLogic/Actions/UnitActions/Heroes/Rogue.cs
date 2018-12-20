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
    public partial class Rogue
    {
        internal class PoisonCloud : BasicAttack
        {
            internal PoisonCloud(Rogue rogue)
                : base(rogue, "Poison Cloud", 0, 1, 100, 100, DamageType.Poison, 2, 2, 2, 3)
            {
            }

            public override void EvaluatePossible(HexGrid hexGrid)
            {
                //TODO find cases where this is not possible
                IsPossible = true;
            }

            public override ActionCategory ActionCategory
            {
                get { return ActionCategory.Traps; }
            }

            public override GenericNetworkAction ExecuteAction(HexGrid hexGrid, Hex[] hexes)
            {
                return new SpawnUnitNetwork(hexGrid.units.getByType2(GenericUnit), hexes, new Trap(this), ActionType.PrimaryAction);
            }
        }

        internal class Tripwire : GenericAttack
        {
            internal Tripwire(Rogue rogue)
                : base(rogue, "Tripwire", 0, 1, 100, 100, DamageType.Magic, 0, 1, 1, 1)
            {
            }

            public override void EvaluatePossible(HexGrid hexGrid)
            {
                //TODO find cases where this is not possible
                IsPossible = true;
            }

            public override ActionCategory ActionCategory
            {
                get { return ActionCategory.Traps; }
            }

            public override GenericNetworkAction ExecuteAction(HexGrid hexGrid, Hex[] hexes)
            {
                return new SpawnUnitNetwork(hexGrid.units.getByType2(GenericUnit), hexes, new Trap(this), ActionType.PrimaryAction);
            }
        }

        internal class Lookout : GenericAttack
        {
            internal Lookout(Rogue rogue)
                : base(rogue, "Lookout", 0, 0, 0, 0, DamageType.Magic, 0, 1, 1, 1)
            {
            }

            public override void EvaluatePossible(HexGrid hexGrid)
            {
                IsPossible = true;
            }

            public override ActionCategory ActionCategory
            {
                get { return ActionCategory.Special; }
            }

            public override GenericNetworkAction ExecuteAction(HexGrid hexGrid, Hex[] hexes)
            {
                Hex targetHex = hexGrid.units.getByType2(GenericUnit);
                //TODO MoveNetwork placeholder: allow adding vision instead!
                return new MoveNetwork(targetHex, new []{ targetHex }, ActionType.PrimaryAction);
            }
        }

        internal class Planeshift : GenericAttack
        {
            internal Planeshift(Rogue rogue)
                : base(rogue, "Planeshift", 0, 0, 0, 0, DamageType.Magic, 0, 1, 1, 1)
            {
            }

            public override void EvaluatePossible(HexGrid hexGrid)
            {
                IsPossible = true;
            }

            public override ActionCategory ActionCategory
            {
                get { return ActionCategory.Special; }
            }

            public override GenericNetworkAction ExecuteAction(HexGrid hexGrid, Hex[] hexes)
            {
                Hex targetHex = hexGrid.units.getByType2(GenericUnit);
                //TODO MoveNetwork placeholder: allow switching to shadow plane
                return new MoveNetwork(targetHex, new []{ targetHex }, ActionType.PrimaryAction);
            }
        }
    }
}

