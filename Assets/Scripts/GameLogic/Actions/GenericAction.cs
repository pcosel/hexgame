using System;
using System.Collections.Generic;

using UnityEngine;

using Hexlibrary;
using Graphics;
using GameLogic.Actions.Network;
using GameLogic.Units;
using GameLogic.Units.Heroes;


namespace GameLogic.Actions
{
    /// <summary>
    /// A generic action to explain the action-unit concept.
    /// </summary>
    [Serializable]
    public abstract class GenericAction
    {

        /// <summary>
        /// The icon for this action.
        /// </summary>
        ActionSprite Sprite;

        /// <summary>
        /// If the action can be executed given the last game state that was checked using evaluatePossible.
        /// </summary>
        public bool IsPossible { get; protected set; }

        /// <summary>
        /// The type of the action. Static because it is the same for all instances of this action.
        /// </summary>
        public abstract ActionType ActionType { get; }

        /// <summary>
        /// The type of category to be shown in the UI.
        /// </summary>
        /// <value>The action category.</value>
        public abstract ActionCategory ActionCategory { get; }

        /// <summary>
        /// The unit this action belongs to. May only be set using the constructor to avoid reassigning actions to other units.
        /// </summary>
        protected GenericUnit GenericUnit;

        /// <summary>
        /// Initializes a new instance of the <see cref="GenericAction"/> class.
        /// </summary>
        /// <param name="genericUnit_">The unit this action belongs to.</param>
        protected GenericAction(GenericUnit genericUnit_)
        {
            if (genericUnit_ == null)
            {
                throw new System.ArgumentException("The Parameter may not be null.", "genericUnit_");
            }
            GenericUnit = genericUnit_;
        }

        public bool GetPossible()
        {
            return IsPossible;
        }

        /// <summary>
        /// Checks if this action can be executed given the current hexGrid and this actions genericUnit and sets its possible field accordingly.
        /// </summary>
        /// <param name="hexGrid">The grid handler representing the current game state.</param>
        public abstract void EvaluatePossible(HexGrid hexGrid);

        /// <summary>
        /// Prepares the action on the given hexGrid by displaying valid target hexes/units.
        /// </summary>
        /// <returns>All valid target hexes for this action.</returns>
        /// <param name="hexGrid">The grid handler representing the current game state.</param>
        public abstract List<Hex> PrepareAction(HexGrid hexGrid);

        /// <summary>
        /// Executes this action on the given hex on the given grid.
        /// </summary>
        /// <param name="hexGrid">The grid handler representing the current game state.</param>
        /// <param name="hexes">The target hexes for this action.</param>
        public abstract GenericNetworkAction ExecuteAction(HexGrid hexGrid, Hex[] hexes);
    }
}