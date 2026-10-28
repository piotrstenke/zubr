using System.Collections.Immutable;
using System.Text;

namespace Zubr.Symbols;

public sealed class ArrayTypeSymbol : TypeSymbol
{
	public override TypeKind Kind => TypeKind.Array;

	public override Accessibility Accessibility => ElementType.Accessibility;

	public int Rank { get; }

	public NamedTypeSymbol ElementType { get; }

	internal ArrayTypeSymbol(
		NamedTypeSymbol elementType,
		int rank
	) : base(GetName(elementType, rank))
	{
		ElementType = elementType;
		Rank = rank;
	}

	private static string GetName(NamedTypeSymbol elementType, int rank)
	{
		if(rank == 1)
		{
			return $"{elementType}[]";
		}

		StringBuilder sb = new();
		sb.Append(elementType.ToString());
		sb.Append('[');

		int length = rank + 1;
		for (int i = 1; i < length; i++)
		{
			sb.Append(',');
		}

		sb.Append(']');

		return sb.ToString();
	}
}
