using System;

namespace Hexlibrary {

	[Serializable]
	public struct Point{
		public readonly double x, y;

		public Point (double x_, double y_) {
			x = x_;
			y = y_;
		}
	}
}
