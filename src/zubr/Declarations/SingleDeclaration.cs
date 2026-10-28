using Zubr.Collections;
using Zubr.Diagnostics;
using Zubr.Text;

namespace Zubr.Declarations;

internal abstract class SingleDeclaration : Declaration
{
	public SyntaxRef Ref { get; }

	public Location Location => Ref.Location;

	public Location NameLocation { get; }

	public new ConstArray<SingleDeclaration> Children => GetChildren();

	public ConstArray<Diagnostic> Diagnostics { get; }

	protected SingleDeclaration(
		DeclarationKind kind,
		string name,
		SyntaxRef @ref,
		Location nameLocation,
		ConstArray<Diagnostic> diagnostics
	) : base(name, kind)
	{
		Ref = @ref;
		NameLocation = nameLocation;
		Diagnostics = diagnostics;
	}

	protected sealed override ConstArray<Declaration> GetChildrenCore()
	{
		return ConstArray<Declaration>.CastUp(GetChildren());
	}

	protected abstract ConstArray<SingleDeclaration> GetChildren();
}
