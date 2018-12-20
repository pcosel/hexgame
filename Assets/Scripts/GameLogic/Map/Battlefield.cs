using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using Hexlibrary;

namespace GameLogic.Map
{
	[Serializable]
	public class Battlefield
	{
		public DictionaryHexHexCellData map { get; }

		public BidirectionalDictionaryHexGenericUnit units { get; }

		public Layout layout { get; }
	
		public Battlefield(DictionaryHexHexCellData mapToSet, BidirectionalDictionaryHexGenericUnit unitsToSet, Layout layoutToSet)
		{
			map = mapToSet;
			units = unitsToSet;
			layout = layoutToSet;
		}

		public Hex unitToHex(Point point)
		{
			return layout.unitToHex (point).hexRound ();
		}

		public Point hexToUnit(Hex hex)
		{
			return layout.hexToUnit (hex);
		}
	}
}
