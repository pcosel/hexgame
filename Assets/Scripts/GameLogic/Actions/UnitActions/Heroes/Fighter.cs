using System;
using System.Collections.Generic;

using GameLogic.Actions;
using GameLogic.Actions.Network;
using Hexlibrary;

namespace GameLogic.Units.Heroes
{
    public partial class Fighter
    {

        internal class PushBack : GenericAttack
        {
            internal PushBack(Fighter fighter)
                : base(fighter, "Push back", 0, 2, 75, 0, DamageType.Physical, 0, 1, 1, 1)
            {
            }

            public override ActionCategory ActionCategory
            {
                get { return ActionCategory.CombatManeuvers; }
            }

            //TODO: find out how to get the next best pushback hex
            private Hex GetNewPosition(HexGrid hexGrid, Hex caster, Hex target)
            {
                //                List<Hex> possibleHexes = hexGrid.getHexesInMovementRange(target, 1);
                //
                //                if (possibleHexes.Count > 1)
                //                {
                //                    
                //                }
                //                new Move(hexGrid.units.getByType2(target));
                return target;
            }

            public override void EvaluatePossible(HexGrid hexGrid)
            {
                IsPossible = hexGrid.getHexesInMovementRange(hexGrid.units.getByType2(this.GenericUnit), 1).Length > 1;
            }

            public override GenericNetworkAction ExecuteAction(HexGrid hexGrid, Hex[] hexes)
            {
                return new MoveNetwork(hexes[0], new [] { GetNewPosition(hexGrid, hexGrid.units.getByType2(this.GenericUnit), hexes[0]) }, ActionType.PrimaryAction);
            }

        }

        internal class ChangeStance : GenericAction
        {
            public Stance Stance { get; set; }

            internal ChangeStance(Fighter genericUnit_)
                : base(genericUnit_)
            {
                this.Stance = genericUnit_.Stance;
            }

            public override void EvaluatePossible(HexGrid hexGrid)
            {
                //TODO find cases where this is not possible (stun etc.)
                IsPossible = true;
            }

            public override ActionType ActionType
            {
                get { return ActionType.SecondaryAction; }
            }

            public override ActionCategory ActionCategory
            {
                get { return ActionCategory.Special; }
            }

            public override GenericNetworkAction ExecuteAction(HexGrid hexGrid, Hex[] hexes)
            {
                return new ChangeStanceNetwork(Stance, hexGrid.units.getByType2(this.GenericUnit), hexes, ActionType);
            }

            public override List<Hex> PrepareAction(HexGrid hexGrid)
            {
                //return current field
                return new List<Hex> { hexGrid.units.getByType2(this.GenericUnit) };
            }
        }
    }
}

