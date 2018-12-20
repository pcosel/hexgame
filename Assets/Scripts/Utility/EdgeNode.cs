using System.Collections.Generic;

public class EdgeNode<Type> {

	Dictionary<Type, EdgeNode<Type>> edges = new Dictionary<Type, EdgeNode<Type>> ();

	public EdgeNode () {

	}

	public EdgeNode (Dictionary<Type, EdgeNode<Type>> edges_) {
		edges = edges_;
	}

	public Dictionary<Type, EdgeNode<Type>> getEdges() {
		return edges;
	}
}
