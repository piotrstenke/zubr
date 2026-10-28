using Zubr.Text;

namespace Zubr.Syntax;

public sealed class TopQualifiedNameSyntax : NameSyntax
{
	public Token Top { get; }

	public Token ColonColon { get; }

	public SimpleNameSyntax Name { get; }

	internal TopQualifiedNameSyntax(
		SyntaxTree tree,
		TextSpan span,
		Token top,
		Token colonColon,
		SimpleNameSyntax name
	) : base(tree, span, SyntaxKind.TopQualifiedName)
	{
		Top = top;
		ColonColon = colonColon;
		Name = name;

		SetParent(name);
	}

	public override string ToString()
	{
		return $"{Top}{ColonColon}{Name}";
	}

	public override void Accept(SyntaxVisitor visitor) => visitor.VisitTopQualifiedName(this);

	public override TResult? Accept<TResult>(SyntaxVisitor<TResult> visitor) where TResult : default => visitor.VisitTopQualifiedName(this);

	internal override SimpleNameSyntax GetUnqualifiedName()
	{
		return Name;
	}
}
