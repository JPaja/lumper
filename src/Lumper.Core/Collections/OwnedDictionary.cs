//Derived from AsmResolver.Collections under MIT License
//https://github.com/Washi1337/AsmResolver/blob/master/src/AsmResolver/Collections/OwnedCollection.cs
//
using System.Collections;
using System.Diagnostics;
using System.Diagnostics.CodeAnalysis;

namespace Lumper.Core.Collections;

/// <summary>
/// Represents an dictionary where each value is owned by some object, and prevents the element from being
/// added to any other instance of the collection or dictionary.
/// </summary>
/// <typeparam name="TOwner">The type of the owner object.</typeparam>
/// <typeparam name="TKey">The type of dictionary key.</typeparam>
/// <typeparam name="TValue">The type of elements to store.</typeparam>
[DebuggerDisplay("Count = {" + nameof(Count) + "}")]
public class OwnedDictionary<TOwner, TKey, TValue> : IDictionary<TKey, TValue>
    where TValue : class, IOwnedElement<TOwner>
    where TOwner : class
{
    private readonly Dictionary<TKey, TValue> _dictionary = new();

    /// <summary>
    /// Creates a new empty dictionary that is owned by an object.
    /// </summary>
    /// <param name="owner">The owner of the collection.</param>
    public OwnedDictionary(TOwner owner)
    {
        Owner = owner ?? throw new ArgumentNullException(nameof(owner));
    }

    /// <summary>
    /// Gets the owner of the collection.
    /// </summary>
    public TOwner Owner
    {
        get;
    }

    /// <summary>
    /// Verifies that the provided item is not null and not added to another collection or dictionary.
    /// </summary>
    /// <param name="item">The item to verify.</param>
    /// <exception cref="ArgumentNullException">Occurs when the item is null.</exception>
    /// <exception cref="ArgumentException">Occurs when the item is already owned by another collection or dictionary.</exception>
    protected void AssertNotNullAndHasNoOwner(TValue? item)
    {
        if (item is null)
            throw new ArgumentNullException(nameof(item));
        if (item.Owner != null && item.Owner != Owner)
            throw new ArgumentException("Item is already added to another collection or dictionary.");
    }

    public TValue this[TKey key]
    {
        get => _dictionary[key];
        set
        {
            AssertNotNullAndHasNoOwner(value);
            if (_dictionary.TryGetValue(key, out var oldValue))
                oldValue.Owner = null;
            value.Owner = Owner;
            _dictionary[key] = value;
        }
    }

    public ICollection<TKey> Keys => _dictionary.Keys;

    public ICollection<TValue> Values => _dictionary.Values;

    public int Count => _dictionary.Count;

    public bool IsReadOnly => false;

    public void Add(TKey key, TValue value)
    {
        AssertNotNullAndHasNoOwner(value);
        value.Owner = Owner;
        _dictionary.Add(key, value);
    }

    public void Add(KeyValuePair<TKey, TValue> item) => Add(item.Key, item.Value);

    public void Clear()
    {
        foreach (var value in _dictionary.Values)
            value.Owner = null;
        _dictionary.Clear();
    }

    public bool Contains(KeyValuePair<TKey, TValue> item) => _dictionary.Contains(item);

    public bool ContainsKey(TKey key) => _dictionary.ContainsKey(key);

    public void CopyTo(KeyValuePair<TKey, TValue>[] array, int arrayIndex)
        => ((IDictionary<TKey, TValue>)_dictionary).CopyTo(array, arrayIndex);

    public IEnumerator<KeyValuePair<TKey, TValue>> GetEnumerator()
        => ((IDictionary<TKey, TValue>)_dictionary).GetEnumerator();

    public bool Remove(TKey key)
    {
        if (_dictionary.TryGetValue(key, out var value))
        {
            value.Owner = null;
            _dictionary.Remove(key);
            return true;
        }

        return false;
    }

    public bool Remove(KeyValuePair<TKey, TValue> item)
    {
        if (((IDictionary<TKey, TValue>)_dictionary).Remove(item))
        {
            item.Value.Owner = null;
            return true;
        }

        return false;
    }

    public bool TryGetValue(TKey key, [MaybeNullWhen(false)] out TValue value)
        => _dictionary.TryGetValue(key, out value);

    IEnumerator IEnumerable.GetEnumerator()
        => ((IEnumerable)_dictionary).GetEnumerator();
}
