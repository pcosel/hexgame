using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Hexlibrary {

	[Serializable]
	public class HexCellData {

		[SerializeField]
		public SerializableColor colour { get; set; }

		public bool passable { get; set; }

		public HexCellData(Color colour_, bool passable_) {
			colour = colour_;
			passable = passable_;
		}

		public bool Equals(HexCellData other) {
			if (other == null) {
				return false;
			}

			return (colour.Equals (other.colour) && passable.Equals (other.passable));
		}

		public override bool Equals (System.Object obj) {
			if (obj == null) {
				return false;
			}

			if (obj is Hex) {
				Hex other = (Hex)obj;
				return Equals (other);
			}

			return false;
		}

		public override int GetHashCode() {
			int hash = 17;
			hash = hash * 31 + colour.GetHashCode ();
			hash = hash * 31 + passable.GetHashCode ();
			return hash;
		}

		public static bool operator == (HexCellData data1, HexCellData data2) {
			if (System.Object.ReferenceEquals(data1, data2))
			{
				return true;
			}

			if (((object)data1 == null) || ((object)data2 == null))
			{
				return false;
			}

			return data1.Equals (data2);
		}

		public static bool operator != (HexCellData data1, HexCellData data2) {
			if (System.Object.ReferenceEquals(data1, data2))
			{
				return false;
			}

			if (((object)data1 == null) || ((object)data2 == null))
			{
				return true;
			}

			return !data1.Equals (data2);
		}
	}
}