using System;
using System.Collections;
using System.Collections.Generic;

using UnityEngine;

/// <summary>
/// A bidirectional dictionary. Each key-to-key relationship may only exist once.
/// </summary>
[Serializable]
public class BidirectionalDictionary<Type1, Type2> : IEnumerable
{

    [SerializeField]
    SerializableDictionary<Type1, Type2> oneToTwo = new SerializableDictionary<Type1, Type2>();
    [SerializeField]
    SerializableDictionary<Type2, Type1> twoToOne = new SerializableDictionary<Type2, Type1>();

    /// <summary>
    /// Adds the key1 key2 pair to the dictionary if no key is already in the dictionary.
    /// </summary>
    /// <param name="key1">The first key to add.</param>
    /// <param name="key2">The second key to add.</param>
    public void put(Type1 key1, Type2 key2)
    {
        if (oneToTwo.ContainsKey(key1))
        {
            throw new System.ArgumentException("Parameter is already contained in the dictionary.", "key1");
        }
        if (twoToOne.ContainsKey(key2))
        {
            throw new System.ArgumentException("Parameter is already contained in the dictionary.", "key2");
        }

        oneToTwo.Add(key1, key2);
        twoToOne.Add(key2, key1);
    }

    /// <summary>
    /// Addes the key1 key2 pair to the dicionary. If entries containing either key exist they will be removed.
    /// </summary>
    /// <param name="key1">The first key to add.</param>
    /// <param name="key2">The second key to add.</param>
    public void forcePut(Type1 key1, Type2 key2)
    {
        removeByType1(key1);
        removeByType2(key2);

        put(key1, key2);
    }

    /// <summary>
    /// Returns the key associated with key1. Returns null if there is no entry for key1.
    /// </summary>
    /// <returns>The key associated with key1 or null if there is no entry for key1.</returns>
    /// <param name="key1">The key to look up.</param>
    public Type2 getByType1(Type1 key1)
    {
        if (oneToTwo.ContainsKey(key1))
        {
            return oneToTwo[key1];
        }
        return default(Type2);
    }

    /// <summary>
    /// Returns the key associated with key2. Returns null if there is no entry for key2.
    /// </summary>
    /// <returns>The key associated with key2 or null if there is no entry for key2.</returns>
    /// <param name="key2">The key to look up.</param>
    public Type1 getByType2(Type2 key2)
    {
        if (twoToOne.ContainsKey(key2))
        {
            return twoToOne[key2];
        }
        return default(Type1);
    }

    /// <summary>
    /// Returns all first keys.
    /// </summary>
    /// <returns>All first keys in the dictionary.</returns>
    public List<Type1> getAllType1()
    {
        List<Type1> list = new List<Type1>();

        foreach (KeyValuePair<Type1, Type2> entry in oneToTwo)
        {
            list.Add(entry.Key);
        }
        return list;
    }

    /// <summary>
    /// Returns all second keys.
    /// </summary>
    /// <returns>All second keys in the dictionary.</returns>
    public List<Type2> getAllType2()
    {
        List<Type2> list = new List<Type2>();

        foreach (KeyValuePair<Type2, Type1> entry in twoToOne)
        {
            list.Add(entry.Key);
        }
        return list;
    }

    /// <summary>
    /// Removes the entry for key1 if it exists.
    /// </summary>
    /// <param name="key1">The key whose entry should be removed.</param>
    public void removeByType1(Type1 key1)
    {
        if (oneToTwo.ContainsKey(key1))
        {
            twoToOne.Remove(oneToTwo[key1]);
            oneToTwo.Remove(key1);
        }
        return;
    }

    /// <summary>
    /// Removes the entry for key2 if it exists.
    /// </summary>
    /// <param name="key2">The key whose entry should be removed.</param>
    public void removeByType2(Type2 key2)
    {
        if (twoToOne.ContainsKey(key2))
        {
            oneToTwo.Remove(twoToOne[key2]);
            twoToOne.Remove(key2);
        }
        return;
    }

    /// <summary>
    /// The number of pairs stored in this dictionary.
    /// </summary>
    public int Count()
    {
        return oneToTwo.Count;
    }

    /// <summary>
    /// Deletes all entries in this dictionary.
    /// </summary>
    public void Clear()
    {
        oneToTwo.Clear();
        twoToOne.Clear();
    }

    /// <summary>
    /// Returns the enumerator as IEnumerator.
    /// </summary>
    /// <returns>The enumerator as IEnumerator.</returns>
    IEnumerator IEnumerable.GetEnumerator()
    {
        return (IEnumerator)GetEnumerator();
    }

    /// <summary>
    /// Returns an enumerator over the BidirectionalDictionary.
    /// </summary>
    /// <returns>An enumerator over the BidirectionalDictionary.</returns>
    public BidirectionalDictionaryEnumerator GetEnumerator()
    {
        return new BidirectionalDictionaryEnumerator(oneToTwo);
    }

    /// <summary>
    /// An enumerator for the BidirectionalDictionary
    /// </summary>
    public class BidirectionalDictionaryEnumerator : IEnumerator
    {

        List<KeyValuePair<Type1, Type2>> entries = new List<KeyValuePair<Type1, Type2>>();

        int position = -1;

        /// <summary>
        /// Initializes a new instance of the BidirectionalDictionary class.
        /// </summary>
        /// <param name="dictionary">The BidirectionalDictionary to create an enumerator from.</param>
        public BidirectionalDictionaryEnumerator(Dictionary<Type1, Type2> dictionary)
        {
            foreach (KeyValuePair<Type1, Type2> entry in dictionary)
            {
                entries.Add(entry);
            }
        }

        /// <summary>
        /// Moves the enumerator to the next entry. Returns if there is such an entry.
        /// </summary>
        /// <returns>If the next entry exists.</returns>
        public bool MoveNext()
        {
            position++;
            return (position < entries.Count);
        }

        /// <summary>
        /// Reset this instance.
        /// </summary>
        public void Reset()
        {
            position = -1;
        }

        /// <summary>
        /// Returns the current object of this enumerator.
        /// </summary>
        /// <value>The current objectof this enumerator.</value>
        object IEnumerator.Current
        {
            get
            {
                return Current;
            }
        }

        /// <summary>
        /// Returns the current KeyValuePair of this enumerator.
        /// </summary>
        /// <value>The current KeyValuePair of this enumerator.</value>
        public KeyValuePair<Type1, Type2> Current
        {
            get
            {
                try
                {
                    return entries[position];
                }
                catch (IndexOutOfRangeException)
                {
                    throw new InvalidOperationException();
                }
            }
        }
    }
}