using Zubr.Collections;

namespace Zubr.Declarations;

internal abstract class Declaration
{
	public DeclarationKind Kind { get; }

	public string Name { get; }

	public ConstArray<Declaration> Children => GetChildrenCore();

	protected Declaration(string name, DeclarationKind kind)
	{
		Name = name;
		Kind = kind;
	}

	protected abstract ConstArray<Declaration> GetChildrenCore();
}
