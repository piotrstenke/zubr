using System.Collections.Immutable;

namespace Zubr.Declarations;

internal abstract class Declaration
{
	public DeclarationKind Kind { get; }

	public string Name { get; }

	public ImmutableArray<Declaration> Children => GetChildrenCore();

	protected Declaration(string name, DeclarationKind kind)
	{
		Name = name;
		Kind = kind;
	}

	protected abstract ImmutableArray<Declaration> GetChildrenCore();
}
