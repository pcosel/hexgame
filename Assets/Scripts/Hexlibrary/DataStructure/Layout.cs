using System;
using System.Collections;
using System.Collections.Generic;

namespace Hexlibrary {

	[Serializable]
	public class Layout {
		public static Orientation layout_pointy = new Orientation (
			Math.Sqrt(3.0),  Math.Sqrt(3.0) / 2.0, 0.0, 3.0 / 2.0,
			Math.Sqrt(3.0) / 3.0, -1.0 / 3.0, 0.0, 2.0 / 3.0,
			0.5);

		public static Orientation layout_flat = new Orientation (
			3.0 / 2.0, 0.0, Math.Sqrt(3.0) / 2.0, Math.Sqrt(3.0),
			2.0 / 3.0, 0.0, -1.0 / 3.0, Math.Sqrt(3.0) / 3.0,
			0.0);

		readonly Orientation orientation;
		readonly Point size;
		readonly Point origin;

		public Layout (Orientation orientation_, Point size_, Point origin_) {
			orientation = orientation_;
			size = size_;
			origin = origin_;
		}

		public Point hexToUnit (Hex hex) {
			double x = (orientation.f0 * hex.getQ () + orientation.f1 * hex.getR ()) * size.x;
			double y = (orientation.f2 * hex.getQ () + orientation.f3 * hex.getR ()) * size.y;
			return new Point (x + origin.x, y + origin.y);
		}

		public FractionalHex unitToHex (Point p) {
			Point pt = new Point ((p.x - origin.x) / size.x, (p.y - origin.y) / size.x);
			double q = orientation.b0 * pt.x + orientation.b1 * pt.y;
			double r = orientation.b2 * pt.x + orientation.b3 * pt.y;
			return new FractionalHex (q, r);
		}

		Point hexCornerOffset(int corner) {
			double angle = 2.0 * Math.PI * (orientation.start_angle + corner) / 6;
			return new Point (size.x * Math.Cos (angle), size.y * Math.Sin (angle));
		}

		public List<Point> polygonCorners(Hex hex) {
			List<Point> corners = new List<Point> ();
			Point center = hexToUnit (hex);
			for (int i = 0; i < 6; i++) {
				Point offset = hexCornerOffset (i);
				corners.Add (new Point (center.x + offset.x, center.y + offset.y));
			}
			return corners;
		}
	}
}