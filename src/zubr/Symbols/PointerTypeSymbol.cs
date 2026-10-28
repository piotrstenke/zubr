using System.Collections.Immutable;

namespace Zubr.Symbols;

public sealed class PointerTypeSymbol : TypeSymbol
{
	public override TypeKind Kind => TypeKind.Pointer;

	public override Accessibility Accessibility => TargetType.Accessibility;

	public TypeSymbol TargetType { get; }

	internal PointerTypeSymbol(TypeSymbol targetType) : base(GetName(targetType))
	{
		TargetType = targetType;
	}

	private static string GetName(TypeSymbol targetType)
	{
		return $"{targetType}*";
	}
}
