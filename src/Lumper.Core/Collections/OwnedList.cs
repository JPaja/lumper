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
public class OwnedList<TOwner, TItem> : IList<TItem>
    where TItem : class, IOwnedElement<TOwner>
    where TOwner : class
{
    private readonly List<TItem> _items = new();

    /// <summary>
    /// Creates a new empty collection that is owned by an object.
    /// </summary>
    /// <param name="owner">The owner of the collection.</param>
    public OwnedList(TOwner owner)
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
    public int Count => _items.Count;

    /// <summary>
    /// Gets the underlying list.
    /// </summary>
    public IList<TItem> Items => _items;

    public bool IsReadOnly => throw new NotImplementedException();

    /// <inheritdoc />
    public void Add(TItem item) => Insert(Count, item);

    /// <summary>
    /// Appends the elements of a collection to the end of the <see cref="LazyList{TItem}"/>.
    /// </summary>
    /// <param name="items">The items to append.</param>
    public void AddRange(IEnumerable<TItem> items) => InsertRange(_items.Count, items);

    /// <inheritdoc />
    public void Clear()
    {
        foreach (var item in _items)
            item.Owner = null;
        _items.Clear();
    }

    /// <inheritdoc />
    public bool Contains(TItem item) => _items.Contains(item);

    /// <inheritdoc />
    public void CopyTo(TItem[] array, int arrayIndex) => _items.CopyTo(array, arrayIndex);

    /// <inheritdoc />
    public bool Remove(TItem item)
    {
        int index = _items.IndexOf(item);
        if (index == -1)
            return false;
        RemoveAt(index);
        return true;
    }

    /// <inheritdoc />
    public int IndexOf(TItem item) => _items.IndexOf(item);

    /// <inheritdoc />
    public void Insert(int index, TItem item)
    {
        AssertNotNullAndHasNoOwner(item);
        item.Owner = Owner;
        _items.Insert(index, item);
    }

    /// <summary>
    /// Inserts the elements of a collection into the <see cref="LazyList{TItem}"/> at the provided index.
    /// </summary>
    /// <param name="index">The starting index to insert the items in.</param>
    /// <param name="items">The items to insert.</param>
    public void InsertRange(int index, IEnumerable<TItem> items)
    {
        var elements = items.ToList();
        foreach (var item in elements)
            AssertNotNullAndHasNoOwner(item);
        foreach (var item in elements)
            item.Owner = Owner;
        _items.InsertRange(index, items);
    }

    /// <inheritdoc />
    public void RemoveAt(int index)
    {
        var item = this[index];
        item.Owner = null;
        _items.RemoveAt(index);
    }

    /// <inheritdoc />
    public IEnumerator<TItem> GetEnumerator() => _items.GetEnumerator();

    /// <inheritdoc />
    IEnumerator IEnumerable.GetEnumerator() => _items.GetEnumerator();
}
