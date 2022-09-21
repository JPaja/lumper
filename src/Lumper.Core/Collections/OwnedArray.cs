//Derived from AsmResolver.Collections under MIT License
//https://github.com/Washi1337/AsmResolver/blob/master/src/AsmResolver/Collections/OwnedCollection.cs
//
using System.Collections;
using System.Diagnostics;

namespace Lumper.Core.Collections;

/// <summary>
/// Represents an indexed collection where each element is owned by some object, and prevents the element from being
/// added to any other instance of the collection or dictionary.
/// </summary>
/// <typeparam name="TOwner">The type of the owner object.</typeparam>
/// <typeparam name="TItem">The type of elements to store.</typeparam>
[DebuggerDisplay("Count = {" + nameof(Count) + "}")]
public class OwnedArray<TOwner, TItem> : IEnumerable<TItem>
    where TItem : class, IOwnedElement<TOwner>
    where TOwner : class
{
    private readonly TItem[] _items;

    /// <summary>
    /// Creates a new empty collection that is owned by an object.
    /// </summary>
    /// <param name="owner">The owner of the collection.</param>
    public OwnedArray(TOwner owner, int size)
    {
        Owner = owner ?? throw new ArgumentNullException(nameof(owner));
        _items = new TItem[size];
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
    protected void AssertNotNullAndHasNoOwner(TItem? item)
    {
        if (item is null)
            throw new ArgumentNullException(nameof(item));
        if (item.Owner != null && item.Owner != Owner)
            throw new ArgumentException("Item is already added to another collection or dictionary.");
    }

    public TItem this[int index]
    {
        get => _items[index];
        set
        {
            AssertNotNullAndHasNoOwner(value);
            _items[index].Owner = null;
            value.Owner = Owner;
            _items[index] = value;
        }
    }

    /// <inheritdoc />
    public int Count => _items.Length;

    /// <summary>
    /// Gets the underlying list.
    /// </summary>
    public TItem[] Items => _items;

    public bool IsReadOnly => throw new NotImplementedException();

    /// <inheritdoc />
    public bool Contains(TItem item) => _items.Contains(item);

    /// <inheritdoc />
    public void CopyTo(TItem[] array, int arrayIndex) => _items.CopyTo(array, arrayIndex);

    /// <inheritdoc />
    IEnumerator IEnumerable.GetEnumerator() => _items.GetEnumerator();

    /// <inheritdoc />
    public IEnumerator<TItem> GetEnumerator() => _items.AsEnumerable().GetEnumerator();
}
