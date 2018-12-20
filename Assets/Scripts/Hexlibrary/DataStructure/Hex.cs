using System;

namespace Hexlibrary {
	[Serializable]
	public class Hex {
		private int[] v;

		private int hashCode;

		public Hex(int q, int r) {
			v = new int[3];
			v [0] = q;
			v [1] = r;
			v [2] = -q - r;
			computeHashCode ();
		}

		private Hex(int q, int r, int s) {
			v = new int[3];
			v [0] = q;
			v [1] = r;
			v [2] = s;
			computeHashCode ();
		}

		private Hex(int[] hexVector) {
			v = new int[3];
			for (int i = 0; i < 3; i++) {
				v [i] = hexVector [i];
			}
			computeHashCode ();
		}

		public int getQ() {
			return v [0];
		}

		public int getR() {
			return v [1];
		}

		public int getS() {
			return v [2];
		}

		public int[] getQRS() {
			return v;
		}

		public override string ToString ()
		{
			return "[Hex][Q:" + v[0] + ", R:" + v[1] + ", S:" + v[2] + "]";
		}

		public bool Equals (Hex other) {
			if (other == null) {
				return false;
			}

			for (int i = 0; i < 3; i++) {
				if (other.getQRS()[i] != v [i]) {
					return false;
				}
			}

			return true;
		}

		public override bool Equals(object obj) {
			if (obj == null) {
				return false;
			}

			if (obj is Hex) {
				Hex other = (Hex)obj;
				return Equals (other);
			}

			return false;
		}

		void computeHashCode() {
			hashCode = 17;
			for (int i = 0; i < 3; i++) {
				hashCode = hashCode * 31 + v[i] * hashCode;
			}
		}

		public override int GetHashCode() {
			return hashCode;
		}

		public static bool operator == (Hex hex1, Hex hex2) {
			if (System.Object.ReferenceEquals(hex1, hex2))
			{
				return true;
			}
				
			if (((object)hex1 == null) || ((object)hex2 == null))
			{
				return false;
			}

			return hex1.Equals (hex2);
		}

		public static bool operator != (Hex hex1, Hex hex2) {
			if (System.Object.ReferenceEquals(hex1, hex2))
			{
				return false;
			}
				
			if (((object)hex1 == null) || ((object)hex2 == null))
			{
				return true;
			}

			return !hex1.Equals (hex2);
		}

		public static Hex operator + (Hex hex1, Hex hex2) {
			int[] result = new int[3];
			for (int i = 0; i < 3; i++) {
				result [i] = hex1.getQRS () [i] + hex2.getQRS () [i];
			}
			return new Hex (result);
		}

		public static Hex operator - (Hex hex1, Hex hex2) {
			int[] result = new int[3];
			for (int i = 0; i < 3; i++) {
				result [i] = hex1.getQRS () [i] - hex2.getQRS () [i];
			}
			return new Hex (result);
		}

		public static Hex operator * (Hex hex1, int k) {
			int[] result = new int[3];
			for (int i = 0; i < 3; i++) {
				result [i] = hex1.getQRS () [i] * k;
			}
			return new Hex (result);
		}
	}
}