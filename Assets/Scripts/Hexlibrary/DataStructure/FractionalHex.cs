using System;

namespace Hexlibrary {
	public struct FractionalHex {
		private double[] v;

		public FractionalHex(double q, double r) {
			v = new double[3];
			v [0] = q;
			v [1] = r;
			v [2] = -q - r;
		}

		private FractionalHex(double q, double r, double s) {
			v = new double[3];
			v [0] = q;
			v [1] = r;
			v [2] = s;
		}

		private FractionalHex(double[] hexVector) {
			v = new double[3];
			for (int i = 0; i < 3; i++) {
				v [i] = hexVector [i];
			}
		}

		public double getQ() {
			return v [0];
		}

		public double getR() {
			return v [1];
		}

		public double getS() {
			return v [2];
		}

		public double[] getQRS() {
			return v;
		}

		public Hex hexRound() {
			int q = (int) Math.Round(getQ());
			int r = (int) Math.Round(getR());
			int s = (int) Math.Round(getS());
			double q_diff = Math.Abs(q - getQ());
			double r_diff = Math.Abs(r - getR());
			double s_diff = Math.Abs(s - getS());
			if (q_diff > r_diff && q_diff > s_diff) {
				q = -r - s;
			} else if (r_diff > s_diff) {
				r = -q - s;
			} else {
				s = -q - r;
			}
			return new Hex(q, r);
		}

		public bool Equals (FractionalHex other) {
			for (int i = 0; i < 3; i++) {
				if (other.getQRS()[i] != v [i]) {
					return false;
				}
			}
			return true;
		}

		public override bool Equals(object obj) {
			if (obj is FractionalHex) {
				FractionalHex other = (FractionalHex)obj;
				return Equals (other);
			}
			return false;
		}

		public override int GetHashCode() {
			return (int) Math.Round(v [0] * 100 + v [1] * 10 + v [2]);
		}

		public static bool operator == (FractionalHex hex1, FractionalHex hex2) {
			return hex1.Equals (hex2);
		}

		public static bool operator != (FractionalHex hex1, FractionalHex hex2) {
			return !hex1.Equals (hex2);
		}

		public static FractionalHex operator + (FractionalHex hex1, FractionalHex hex2) {
			double[] result = new double[3];
			for (int i = 0; i < 3; i++) {
				result [i] = hex1.getQRS () [i] + hex2.getQRS () [i];
			}
			return new FractionalHex(result);
		}

		public static FractionalHex operator - (FractionalHex hex1, FractionalHex hex2) {
			double[] result = new double[3];
			for (int i = 0; i < 3; i++) {
				result [i] = hex1.getQRS () [i] - hex2.getQRS () [i];
			}
			return new FractionalHex (result);
		}

		public static FractionalHex operator * (FractionalHex hex1, int k) {
			double[] result = new double[3];
			for (int i = 0; i < 3; i++) {
				result [i] = hex1.getQRS () [i] * k;
			}
			return new FractionalHex (result);
		}
	}
}