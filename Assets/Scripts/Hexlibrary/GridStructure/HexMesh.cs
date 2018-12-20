using System.Collections.Generic;
using UnityEngine;
namespace Hexlibrary {
	[RequireComponent (typeof(MeshFilter), typeof(MeshRenderer))]
	public class HexMesh : MonoBehaviour {

		Mesh hexMesh;
		MeshCollider meshCollider;

		List<Vector3> vertices;
		List<int> triangles;
		List<Color> coloures;

		// Use this for initialization
		void Awake () {
			GetComponent<MeshFilter> ().mesh = hexMesh = new Mesh ();
			meshCollider = gameObject.AddComponent<MeshCollider> ();
			hexMesh.name = "Hex Mesh";
			vertices = new List<Vector3> ();
			triangles = new List<int> ();
			coloures = new List<Color> ();
		}

		public void Triangulate (HexGrid hexGrid) {

			Dictionary<Hex, HexCellData> map = hexGrid.map;

			hexMesh.Clear ();
			vertices.Clear ();
			triangles.Clear ();
			coloures.Clear ();

			foreach (KeyValuePair<Hex, HexCellData> entry in map) {
				Triangulate (entry, hexGrid.layout);
			}

			hexMesh.vertices = vertices.ToArray ();
			hexMesh.triangles = triangles.ToArray ();
			hexMesh.colors = coloures.ToArray ();	
			hexMesh.RecalculateNormals ();
			meshCollider.sharedMesh = hexMesh;
		}

		void Triangulate (KeyValuePair<Hex, HexCellData> entry, Layout layout) {
			Vector3 center = GridUtility.pointToVector3 (layout.hexToUnit(entry.Key));

			List<Point> hexCorners = layout.polygonCorners (entry.Key);

			for (int i = 0; i < 6; i++) {
				AddTriangle (center, GridUtility.pointToVector3(hexCorners[(i + 1) % 6]), GridUtility.pointToVector3(hexCorners[i]));
				AddTriangleColour (entry.Value.colour);
			}
		}

		void AddTriangle (Vector3 v1, Vector3 v2, Vector3 v3) {
			int vertexIndex = vertices.Count;
			vertices.Add (v1);
			vertices.Add (v2);
			vertices.Add (v3);
			triangles.Add (vertexIndex);
			triangles.Add (vertexIndex + 1);
			triangles.Add (vertexIndex + 2);
		}

		void AddTriangleColour (Color colour) {
			for (int i = 0; i < 3; i++) {
				coloures.Add (colour);
			}
		}
	}
}