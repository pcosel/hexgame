using System.Collections.Generic;
using UnityEngine;

namespace Hexlibrary {
	public static class GridUtility {

		public static Vector3 pointToVector3 (Point point) {
			Vector3 vector;
			vector.x = (float) point.x;
			vector.y = (float) point.y;
			vector.z = 0;
			return vector;
		}
	}
}