using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

using Hexlibrary;
using GameLogic.Units.Heroes;
using GameLogic.Units.Villains;
using GameLogic.Units;

namespace GameLogic.Map
{
    public static class BattlefieldGenerator
    {
        public static readonly Color defaultColour = Color.white;
        public static readonly Color blockedColour = Color.black;
        public static readonly Color touchedColour = Color.green;

        static Layout defaultLayout = new Layout(Layout.layout_pointy, new Point(1.5, 1.5), new Point(0, 0));

        public static Battlefield emptyDefaultMap()
        {
            DictionaryHexHexCellData map = new DictionaryHexHexCellData();

            int radius = 5;

            for (int q = -radius; q <= radius; q++)
            {
                int r1 = Math.Max(-radius, -q - radius);
                int r2 = Math.Min(radius, -q + radius);
                for (int r = r1; r <= r2; r++)
                {
                    Hex hex = new Hex(q, r);
                    map.Add(hex, new HexCellData(defaultColour, true));
                }
            }

            BidirectionalDictionaryHexGenericUnit units = new BidirectionalDictionaryHexGenericUnit();

            units.put(new Hex(0, 0), new Fighter("fighter"));
            units.put(new Hex(0, 1), new Rogue("rogue"));
            units.put(new Hex(1, 0), new Mage("mage"));
            units.put(new Hex(2, 2), new Assassin("assassin"));

            return new Battlefield(map, units, defaultLayout);
        }

        public static Battlefield randomCircleMap(int radius, List<GenericUnit> unitsToPlace)
        {
            DictionaryHexHexCellData map = new DictionaryHexHexCellData();

            int placableNeeded = unitsToPlace.Count;

            List<Hex> possibleToPlace = new List<Hex>();

            for (int q = -radius; q <= radius; q++)
            {
                int r1 = Math.Max(-radius, -q - radius);
                int r2 = Math.Min(radius, -q + radius);
                for (int r = r1; r <= r2; r++)
                {
                    Hex hex = new Hex(q, r);

                    if (placableNeeded > 0 || UnityEngine.Random.Range(0, 10) != 0)
                    {
                        placableNeeded--;

                        map.Add(hex, new HexCellData(defaultColour, true));
                        possibleToPlace.Add(hex);
                    }
                    else
                    {
                        map.Add(hex, new HexCellData(blockedColour, false));
                    }
                }
            }

            if (unitsToPlace.Count > possibleToPlace.Count)
            {
                throw new ArgumentException("There are too many units for this radius", "");
            }

            BidirectionalDictionaryHexGenericUnit units = new BidirectionalDictionaryHexGenericUnit();

            while (unitsToPlace.Count > 0)
            {
                int location = UnityEngine.Random.Range(0, possibleToPlace.Count);

                units.put(possibleToPlace[location], unitsToPlace[0]);

                unitsToPlace.RemoveAt(0);
                possibleToPlace.RemoveAt(location);
            }

            return new Battlefield(map, units, defaultLayout);
        }
    }
}
