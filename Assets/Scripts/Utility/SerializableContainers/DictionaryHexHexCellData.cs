using System;
using System.Runtime.Serialization;

using Hexlibrary;

[Serializable]
public class DictionaryHexHexCellData : SerializableDictionary<Hex, HexCellData> {
	public DictionaryHexHexCellData(){}

	public DictionaryHexHexCellData(SerializationInfo info, StreamingContext context) : base (info, context){}
}

