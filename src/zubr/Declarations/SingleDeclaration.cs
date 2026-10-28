using System.Collections.Immutable;
using Zubr.Text;

namespace Zubr.Declarations;

internal abstract class SingleDeclaration : Declaration
{
	public SyntaxRef Ref { get; }

	public Location Location => Ref.Location;

	public Location NameLocation { get; }

	public new ImmutableArray<SingleDeclaration> Children => GetChildren();

	protected SingleDeclaration(
		DeclarationKind kind,
		string name,
		SyntaxRef @ref,
		Location nameLocation
	) : base(name, kind)
	{
		Ref = @ref;
		NameLocation = nameLocation;
	}

	protected sealed override ImmutableArray<Declaration> GetChildrenCore()
	{
		return ImmutableArray<Declaration>.CastUp(GetChildren());
	}

	protected abstract ImmutableArray<SingleDeclaration> GetChildren();
}
