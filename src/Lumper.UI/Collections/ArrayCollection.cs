/*
https://github.com/trigger-segfault/GrisaiaSpriteViewer/blob/master/WpfTesting/Model/ArrayCollection.cs

MIT License

Copyright (c) 2019 Robert Jordan

Permission is hereby granted, free of charge, to any person obtaining a copy of this software and associated documentation files (the "Software"), to deal in the Software without restriction, including without limitation the rights to use, copy, modify, merge, publish, distribute, sublicense, and/or sell copies of the Software, and to permit persons to whom the Software is furnished to do so, subject to the following conditions:

The above copyright notice and this permission notice shall be included in all copies or substantial portions of the Software.

THE SOFTWARE IS PROVIDED "AS IS", WITHOUT WARRANTY OF ANY KIND, EXPRESS OR IMPLIED, INCLUDING BUT NOT LIMITED TO THE WARRANTIES OF MERCHANTABILITY, FITNESS FOR A PARTICULAR PURPOSE AND NONINFRINGEMENT. IN NO EVENT SHALL THE AUTHORS OR COPYRIGHT HOLDERS BE LIABLE FOR ANY CLAIM, DAMAGES OR OTHER LIABILITY, WHETHER IN AN ACTION OF CONTRACT, TORT OR OTHERWISE, ARISING FROM, OUT OF OR IN CONNECTION WITH THE SOFTWARE OR THE USE OR OTHER DEALINGS IN THE SOFTWARE.
 */

using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;

namespace Lumper.UI.Collections;

[Serializable]
[DebuggerDisplay("Length = {Count}")]
public class ArrayCollection<T> : IEnumerable<T>
{
	#region Fields

	protected T[] Items { get; }

	#endregion

	#region Constructors

	public ArrayCollection(int length)
	{
		if (length < 0)
			throw new ArgumentOutOfRangeException(nameof(length));
		Items = new T[length];
	}

	public ArrayCollection(T[] array)
	{
		if (array == null)
			throw new ArgumentNullException(nameof(array));
		Items = new T[array.Length];
		Array.Copy(array, Items, Items.Length);
	}

	public ArrayCollection(ICollection<T> collection)
	{
		if (collection == null)
			throw new ArgumentNullException(nameof(collection));
		Items = new T[collection.Count];
		collection.CopyTo(Items, 0);
	}

	public ArrayCollection(IEnumerable<T> collection)
	{
		if (collection == null)
			throw new ArgumentNullException(nameof(collection));
		Items = collection.ToArray();
	}

	#endregion

	#region IList Properties

	public int Count => Items.Length;
	public bool IsSynchronized => false;
	public object SyncRoot => Items.SyncRoot;
	public bool IsReadOnly => false;
	public bool IsFixedSize => true;

	#endregion

	#region Virtual Methods

	protected virtual void SetItem(int index, T item) => Items[index] = item;

	#endregion

	#region Supported IList Implementation

	public T this[int index]
	{
		get => Items[index];
		set
		{
			if (index < 0 || index >= Items.Length)
				throw new ArgumentOutOfRangeException(nameof(index));
			SetItem(index, value);
		}
	}

	public T[] ToArray()
	{
		var array = new T[Items.Length];
		Array.Copy(Items, array, Items.Length);
		return array;
	}

	public void CopyTo(T[] array, int index) => Array.Copy(Items, 0, array, index, Items.Length);
	public void CopyTo(Array array, int index) => Array.Copy(Items, 0, array, index, Items.Length);
	public bool Contains(T item) => Array.IndexOf(Items, item) != -1;
	public int IndexOf(T item) => Array.IndexOf(Items, item);

	public IEnumerator<T> GetEnumerator() => Items.Cast<T>().GetEnumerator();
	IEnumerator IEnumerable.GetEnumerator() => Items.GetEnumerator();

	#endregion
}