using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Linq;
using System.Runtime.CompilerServices;

namespace Zubr;

internal static class CollectionHelpers
{
	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	public static void NewOrClear<T>([NotNull] ref List<T>? list)
	{
		if (list is null)
		{
			list = new();
		}
		else
		{
			list.Clear();
		}
	}

	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	public static void NewOrClear<T>([NotNull] ref List<T>? list, int capacity)
	{
		if (list is null)
		{
			list = new(capacity);
		}
		else
		{
			list.Clear();
		}
	}

	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	public static void InitWithCapacity<T>(ref List<T>? list, IEnumerable<T> collection)
	{
		if(list is null)
		{
			if(collection.TryGetNonEnumeratedCount(out int count))
			{
				list = new(count);
			}
			else
			{
				list = new();
			}
		}
	}

	public static void Add<T>([NotNull] ref List<T>? list, T item, [CallerArgumentExpression(nameof(item))] string? paramName = null)
	{
		ArgumentNullException.ThrowIfNull(item, paramName);

		list ??= new();
		list.Add(item);
	}

	public static void Add<T>([NotNull] ref List<T>? list, IEnumerable<T> items, [CallerArgumentExpression(nameof(items))] string? paramName = null)
	{
		ArgumentNullException.ThrowIfNull(items, paramName);

		list ??= new();
		list.AddRange(items);
	}

	public static void With<T>([NotNull] ref List<T>? list, T item, [CallerArgumentExpression(nameof(item))] string? paramName = null)
	{
		ArgumentNullException.ThrowIfNull(item, paramName);

		NewOrClear(ref list);
		list.Add(item);
	}

	public static void With<T>([NotNull] ref List<T>? list, IEnumerable<T> items, [CallerArgumentExpression(nameof(items))] string? paramName = null)
	{
		ArgumentNullException.ThrowIfNull(items, paramName);

		NewOrClear(ref list);
		list.AddRange(items);
	}
}
