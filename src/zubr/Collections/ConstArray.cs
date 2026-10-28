using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using System.Diagnostics.CodeAnalysis;
using System.Runtime.CompilerServices;

namespace Zubr.Collections;

[CollectionBuilder(typeof(ConstArray), nameof(ConstArray.Create))]
[DebuggerDisplay("Count = {Count}")]
[DebuggerTypeProxy(typeof(ConstArray<>.DebuggerProxy))]
public readonly struct ConstArray<T> : IReadOnlyList<T>, IEquatable<ConstArray<T>>, IStructuralEquatable
{
	public static readonly ConstArray<T> Empty = new(Array.Empty<T>());

	private readonly T[] _elements;

	public bool IsEmpty => _elements.Length == 0;

	public bool IsDefault => _elements is null;

	public bool IsDefaultOrEmpty => IsDefault || IsEmpty;

	public int Length => _elements.Length;

	readonly int IReadOnlyCollection<T>.Count => Length;

	public T this[int index] => _elements[index];

	public ConstArray()
	{
		_elements = Array.Empty<T>();
	}

	internal ConstArray(T[] elements)
	{
		_elements = elements;
	}

	public bool Equals(ConstArray<T> other)
	{
		return other._elements == _elements;
	}

	public override bool Equals([NotNullWhen(true)] object? obj)
	{
		return obj is ConstArray<T> other && Equals(other);
	}

	public override int GetHashCode()
	{
		return _elements?.GetHashCode() ?? 0;
	}

	public Enumerator GetEnumerator()
	{
		return new(_elements);
	}

	public static bool operator ==(ConstArray<T> left, ConstArray<T> right)
	{
		return left.Equals(right);
	}

	public static bool operator !=(ConstArray<T> left, ConstArray<T> right)
	{
		return !(left == right);
	}

	public static ConstArray<T> CastUp<TDerived>(ConstArray<TDerived> self) where TDerived : class?, T
	{
		return new(self._elements);
	}

	public static implicit operator ConstArray<T>(T[] array)
	{
		return ConstArray.Create(array);
	}

	bool IStructuralEquatable.Equals(object? other, IEqualityComparer comparer)
	{
		if(other is null || other is not ConstArray<T> array)
		{
			return false;
		}

		if(_elements == array._elements)
		{
			return true;
		}

		if(_elements is null || array._elements is null)
		{
			return false;
		}

		if(_elements.Length != array.Length)
		{
			return false;
		}

		for (int i = 0; i < _elements.Length; i++)
		{
			if (!comparer.Equals(_elements[i], array[i]))
			{
				return false;
			}
		}

		return true;
	}

	int IStructuralEquatable.GetHashCode(IEqualityComparer comparer)
	{
		return (_elements as IStructuralEquatable)?.GetHashCode(comparer) ?? 0;
	}

	IEnumerator<T> IEnumerable<T>.GetEnumerator()
	{
		return new EnumeratorObj(_elements);
	}

	IEnumerator IEnumerable.GetEnumerator()
	{
		return new EnumeratorObj(_elements);
	}

	public struct Enumerator
	{
		private readonly T[] _array;

		private int _index;

		public readonly T Current => _array[_index];

		internal Enumerator(T[] array)
		{
			_array = array;
			_index = -1;
		}

		public bool MoveNext()
		{
			if(_index >= _array.Length)
			{
				return false;
			}

			_index++;
			return true;
		}
	}

	private sealed class EnumeratorObj : IEnumerator<T>
	{
		private readonly T[] _array;

		private int _index;

		public T Current => _array[_index];

		object? IEnumerator.Current => Current;

		internal EnumeratorObj(T[] array)
		{
			_array = array;
			_index = -1;
		}

		public bool MoveNext()
		{
			if (_index >= _array.Length)
			{
				return false;
			}

			_index++;
			return true;
		}

		public void Reset()
		{
			_index = -1;
		}

		public void Dispose()
		{
		}
	}

	private sealed class DebuggerProxy
	{
		private readonly ConstArray<T> _array;

		public DebuggerProxy(ConstArray<T> array)
		{
			_array = array;
		}

		[DebuggerBrowsable(DebuggerBrowsableState.RootHidden)]
		public T[] Items
		{
			get
			{
				T[] result = new T[_array.Length];

				for (int i = 0; i < result.Length; i++)
				{
					result[i] = _array[i];
				}

				return result;
			}
		}
	}
}

public static class ConstArray
{
	public static ConstArray<T> Create<T>()
	{
		return ConstArray<T>.Empty;
	}

	public static ConstArray<T> Create<T>(T item)
	{
		T[] items = [item];
		return new(items);
	}

	public static ConstArray<T> Create<T>(T item1, T item2)
	{
		T[] items = [item1, item2];
		return new(items);
	}

	public static ConstArray<T> Create<T>(T item1, T item2, T item3)
	{
		T[] items = [item1, item2, item3];
		return new(items);
	}

	public static ConstArray<T> Create<T>(T item1, T item2, T item3, T item4)
	{
		T[] items = [item1, item2, item3, item4];
		return new(items);
	}

	public static ConstArray<T> Create<T>(params ReadOnlySpan<T> items)
	{
		if (items.IsEmpty)
		{
			return ConstArray<T>.Empty;
		}

		return new(items.ToArray());
	}

	public static ConstArray<T> Create<T>(params T[]? items)
	{
		if (items is null || items.Length == 0)
		{
			return ConstArray<T>.Empty;
		}

		T[] array = new T[items.Length];
		Array.Copy(items, array, items.Length);
		return new(array);
	}

	public static ConstArray<T> Create<T>(params IEnumerable<T>? items)
	{
		if (items is null)
		{
			return ConstArray<T>.Empty;
		}

		ArrayBuilder<T> builder = items.ToArrayBuilder();
		return builder.ToConstArray();
	}
}
