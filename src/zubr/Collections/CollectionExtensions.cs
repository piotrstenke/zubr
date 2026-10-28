using System;
using System.Collections.Generic;
using System.Linq;

namespace Zubr.Collections;

public static class CollectionExtensions
{
	public static ConstArray<T> ToConstArray<T>(this IEnumerable<T> collection)
	{
		return ConstArray.Create(collection);
	}

	public static ConstArray<T> ToConstArray<T>(this T[] array)
	{
		return ConstArray.Create(array);
	}

	public static ConstArray<T> ToConstArray<T>(this ReadOnlySpan<T> span)
	{
		return ConstArray.Create(span);
	}

	public static ArrayBuilder<T> ToArrayBuilder<T>(this IEnumerable<T> collection)
	{
		ArrayBuilder<T> builder;

		if (collection.TryGetNonEnumeratedCount(out int count))
		{
			builder = new(count);
			builder.AddRange_NoCapacityCheck(collection);
		}
		else
		{
			builder = new();
			builder.AddRange(collection);
		}

		return builder;
	}

	public static ArrayBuilder<T> ToArrayBuilder<T>(this T[] array)
	{
		ArrayBuilder<T> builder = new(array.Length);
		builder.AddRange(array);
		return builder;
	}

	public static ArrayBuilder<T> ToArrayBuilder<T>(this ReadOnlySpan<T> span)
	{
		ArrayBuilder<T> builder = new(span.Length);
		builder.AddRange(span);
		return builder;
	}

	internal static ConstArray<T> SelfOrEmpty<T>(this ConstArray<T> array)
	{
		return array.IsDefault ? ConstArray<T>.Empty : array;
	}
}
