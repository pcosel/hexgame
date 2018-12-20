using System;
using System.Collections.Generic;

using UnityEngine;

namespace Hexlibrary {
	public static class HexUtility{

		static List<Hex> directions = new List<Hex>{
			new Hex(1, 0),
			new Hex(1, -1),
			new Hex(0, -1),
			new Hex(-1, 0),
			new Hex(-1, 1),
			new Hex(0, 1)};

		public static int hexLenght(Hex hex) {
			double result = 0;
			for (int i = 0; i < 3; i++) {
				result += Math.Abs (hex.getQRS () [i]);
			}
			return (int) Math.Round(result / 2);
		}

		public static int hexDistance(Hex hex1, Hex hex2) {
			return hexLenght (hex1 - hex2);
		}

		public static Hex getDirection(int direction) {
			return directions[direction % 6];
		}

		public static Hex getNeighbour(Hex hex, int direction) {
			return hex + getDirection (direction);
		}

		public static Hex[] getHexesInMovementRange(Hex center, int range, Dictionary<Hex, bool> passableTerrain) {
			List<Hex> results = new List<Hex> ();

			results.Add (center);

			List<List<Hex>> fringes = new List<List<Hex>> ();
			fringes.Add (new List<Hex> () { center });

			for (int k = 1; k <= range; k++) {
				fringes.Add (new List<Hex> ());
				foreach (Hex hex in fringes[k-1]) {
					for (int dir = 0; dir < 6; dir++) {
						Hex neighbor = getNeighbour (hex, dir);
						if (passableTerrain.ContainsKey(neighbor)) {
							if (!results.Contains (neighbor) && passableTerrain [neighbor]) {
								results.Add (neighbor);
								fringes [k].Add (neighbor);
							}
						}
					}
				}
			}

			results.Remove (center);

			return results.ToArray();
		}

		public static EdgeNode<Hex> getHexGraphInMovementRange(Hex center, int range, Dictionary<Hex, bool> passableTerrain) {
			return constructMovementGraph (center, 1, range, passableTerrain);
		}

		static EdgeNode<Hex> constructMovementGraph(Hex center, int currentDepth, int maximumDepth, Dictionary<Hex, bool> passableTerrain) {
			if (currentDepth >= maximumDepth) {
				return new EdgeNode<Hex> ();
			}

			Dictionary<Hex, EdgeNode<Hex>> edges = new Dictionary<Hex, EdgeNode<Hex>> ();

			for (int dir = 0; dir < 6; dir++) {
				Hex neighbor = getNeighbour (center, dir);
				if (passableTerrain.ContainsKey (neighbor)) {
					if (passableTerrain [neighbor]) {
						edges.Add (neighbor, constructMovementGraph(neighbor, currentDepth + 1, maximumDepth, passableTerrain));
					}
				}
			}

			return new EdgeNode<Hex> (edges);
		}
	}
}