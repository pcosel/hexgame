using System;
using System.Collections.Generic;
using System.Runtime.Serialization;
using UnityEngine;

[Serializable]
public class SerializableDictionary<TKey, TValue> : Dictionary<TKey, TValue>, ISerializationCallbackReceiver
{

	[SerializeField]
	List<TKey> keys = new List<TKey> ();

	[SerializeField]
	List<TValue> values = new List<TValue> ();

	public SerializableDictionary(){}

	public SerializableDictionary(SerializationInfo info, StreamingContext context) : base (info, context){}

	public void OnBeforeSerialize ()
	{
		keys.Clear ();
		values.Clear ();

		foreach (KeyValuePair<TKey, TValue> entry in this) {
			keys.Add (entry.Key);
			values.Add (entry.Value);
		}
	}

	public void OnAfterDeserialize ()
	{
		Clear ();

		if(keys.Count != values.Count) {
			throw new Exception (string.Format ("there are {0} keys and {1} values after deserialization. Make sure that both key and value types are serializable."));
		}

		for (int i = 0; i < keys.Count; i++) {
			Add (keys[i], values[i]);
		}
	}
}
