using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;

namespace Zubr.Collections;

[DebuggerDisplay("Count = {Count}")]
[DebuggerTypeProxy(typeof(ArrayBuilder<>.DebuggerProxy))]
public struct ArrayBuilder<T> : IList<T>, IReadOnlyList<T>, IBuilder<ConstArray<T>>
{
	public static ArrayBuilder<T> Default => default;

	private const int DEFAULT_SIZE = 8;

	private T[] _elements;

	private int _count;

	public readonly bool IsEmpty => _count == 0;

	public readonly bool IsDefault => _elements is null;

	public readonly int Length => _count;

	public int Capacity
	{
		readonly get => _elements.Length;
		set
		{
			ArgumentOutOfRangeException.ThrowIfNegative(value);

			if(_elements is null)
			{
				_elements = new T[value];
			}
			else if(value < _elements.Length)
			{
				SetCount(value);
			}
			else if(value > _elements.Length)
			{
				Grow(value);
			}
		}
	}

	readonly int ICollection<T>.Count => Length;

	readonly int IReadOnlyCollection<T>.Count => Length;

	readonly bool ICollection<T>.IsReadOnly => false;

	public readonly T this[int index]
	{
		get
		{
			if (index >= _count)
			{
				throw new IndexOutOfRangeException();
			}

			return _elements[index];
		}
		set
		{
			if (index >= _count)
			{
				throw new IndexOutOfRangeException();
			}

			_elements[index] = value;
		}
	}

	public ArrayBuilder()
	{
		_elements = new T[DEFAULT_SIZE];
	}

	public ArrayBuilder(int capacity)
	{
		_elements = new T[capacity];
	}

	public void Add(T item)
	{
		EnsureCapacity(_count + 1);

		_elements[_count++] = item;
	}

	public void AddRange(T[] items)
	{
		EnsureCapacity(_count + items.Length);

		for (int i = 0; i < items.Length; i++)
		{
			_elements[_count++] = items[i];
		}
	}

	public void AddRange(ReadOnlySpan<T> items)
	{
		EnsureCapacity(_count + items.Length);

		for (int i = 0; i < items.Length; i++)
		{
			_elements[_count++] = items[i];
		}
	}

	public void AddRange(ConstArray<T> items)
	{
		if(items.IsDefaultOrEmpty)
		{
			return;
		}

		EnsureCapacity(_count + items.Length);

		for (int i = 0; i < items.Length; i++)
		{
			_elements[_count++] = items[i];
		}
	}

	public void AddRange(IEnumerable<T> items)
	{
		if(items.TryGetNonEnumeratedCount(out int count))
		{
			EnsureCapacity(_count + count);
			AddRange_NoCapacityCheck(items);
		}
		else
		{
			foreach (T item in items)
			{
				Add(item);
			}
		}
	}

	internal void AddRange_NoCapacityCheck(IEnumerable<T> items)
	{
		foreach (T item in items)
		{
			_elements[_count++] = item;
		}
	}

	public void Init()
	{
		_elements ??= new T[DEFAULT_SIZE];
	}

	public void Init(int capacity)
	{
		_elements ??= new T[capacity];
	}

	public readonly bool Contains(T item)
	{
		return IndexOf(item) >= 0;
	}

	public readonly int IndexOf(T item)
	{
		return Array.IndexOf(_elements, item, 0, _count);
	}

	public void Insert(int index, T item)
	{
		EnsureCapacity(_count + 1);

		if (index < _count)
		{
			Array.Copy(_elements, index, _elements, index + 1, _count - index);
		}

		_count++;
		_elements[index] = item;
	}

	public void RemoveAt(int index)
	{
		if(index < _elements.Length - 1)
		{
			Array.Copy(_elements, index + 1, _elements, index, _count - index - 1);
		}

		_count--;
	}

	public bool Remove(T item)
	{
		int index = IndexOf(item);

		if (index >= 0)
		{
			RemoveAt(index);
			return true;
		}

		return false;
	}

	public void Clear()
	{
		SetCount(0);
	}

	public readonly void CopyTo(T[] array)
	{
		Array.Copy(_elements, 0, array, 0, _count);
	}

	public readonly void CopyTo(T[] array, int arrayIndex)
	{
		Array.Copy(_elements, 0, array, arrayIndex, _count);
	}

	public T[] ToArray()
	{
		if(_elements is null)
		{
			return Array.Empty<T>();
		}

		T[] array = _elements;

		if (array.Length > _count)
		{
			array = new T[_count];
			Array.Copy(_elements, array, _count);
		}

		_elements = Array.Empty<T>();
		_count = 0;

		return array;
	}

	public ConstArray<T> ToConstArray()
	{
		if(_elements is null)
		{
			return ConstArray<T>.Empty;
		}

		T[] array = _elements;

		if(array.Length > _count)
		{
			array = new T[_count];
			Array.Copy(_elements, array, _count);
		}

		_elements = Array.Empty<T>();
		_count = 0;

		return new(array);
	}

	public readonly IEnumerator<T> GetEnumerator()
	{
		for (int i = 0; i < _count; i++)
		{
			yield return _elements[i];
		}
	}

	readonly IEnumerator IEnumerable.GetEnumerator()
	{
		return GetEnumerator();
	}

	ConstArray<T> IBuilder<ConstArray<T>>.Build()
	{
		return ToConstArray();
	}

	private void EnsureCapacity(int expected)
	{
		if (expected >= _elements.Length)
		{
			Grow(Math.Max(_elements.Length * 2, expected));
		}
	}

	private void Grow(int size)
	{
		Array.Resize(ref _elements, size);
	}

	private void SetCount(int count)
	{
		_count = count;

		for (int i = count; i < _elements.Length; i++)
		{
			_elements[i] = default!;
		}
	}

	private sealed class DebuggerProxy
	{
		private readonly ArrayBuilder<T> _builder;

		public DebuggerProxy(ArrayBuilder<T> builder)
		{
			_builder = builder;
		}

		[DebuggerBrowsable(DebuggerBrowsableState.RootHidden)]
		public T[] Items
		{
			get
			{
				T[] result = new T[_builder.Length];

				for (int i = 0; i < result.Length; i++)
				{
					result[i] = _builder[i];
				}

				return result;
			}
		}
	}
}
