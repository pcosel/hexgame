using System.IO;
using System.Runtime.Serialization;
using System.Runtime.Serialization.Formatters.Binary;

using GameLogic.Actions.Network;
using GameLogic.Map;

public static class NetworkUtility {

	public static byte[] serialize(GenericNetworkAction action) {
		MemoryStream stream = new MemoryStream();
		BinaryFormatter binaryFormatter = new BinaryFormatter();
		binaryFormatter.Serialize(stream, action);
		return stream.GetBuffer();
	}

	public static GenericNetworkAction deserialize(byte[] networkSerializedAction) {
		MemoryStream stream = new MemoryStream(networkSerializedAction);
		BinaryFormatter binaryFormatter = new BinaryFormatter();
		return (GenericNetworkAction) binaryFormatter.Deserialize(stream);
	}

	// Should be merged with serialize(GenericNetworkAction action)
	public static byte[] serialize(Battlefield battlefield) {
		MemoryStream stream = new MemoryStream();
		BinaryFormatter binaryFormatter = new BinaryFormatter();
		binaryFormatter.Serialize(stream, battlefield);
		return stream.GetBuffer();
	}

	public static Battlefield deserializeBattlefield(byte[] networkSerializedBattlefield) {
		MemoryStream stream = new MemoryStream(networkSerializedBattlefield);
		BinaryFormatter binaryFormatter = new BinaryFormatter();
		return (Battlefield) binaryFormatter.Deserialize(stream);
	}
}
