using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;

namespace GameLogic.Units
{
    /// <summary>
    /// Factory class for creating sample units based on given enumerations <see cref="HeroType"/> and <see cref="VillainType"/>.
    /// </summary>
    public static class GenericUnitFactory
    {
        private const string HeroNameSpace = "Assets.Scripts.GameLogic.Units.Heroes";
        private const string VillainNameSpace = "Assets.Scripts.GameLogic.Units.Villains";

        /// <summary>
        /// Return unit based corresponding to the given HeroType.
        /// </summary>
        /// <param name="heroType">The class of hero to be instantiated.</param>
        /// <returns>A hero object corresponding to the given HeroType.</returns>
        public static GenericUnit getUnit(HeroType heroType)
        {
            return (GenericUnit)Activator.CreateInstance(Type.GetType(HeroNameSpace + "." + heroType.ToString()));
        }

        /// <summary>
        /// Return unit based corresponding to the given VillainType.
        /// </summary>
        /// <param name="villainType">The class of villain to be instantiated.</param>
        /// <returns>A villain object corresponding to the given VillainType.</returns>
        public static GenericUnit getUnit(VillainType villainType)
        {
            return (GenericUnit)Activator.CreateInstance(Type.GetType(VillainNameSpace + "." + villainType.ToString())); ;
        }
    }
}
