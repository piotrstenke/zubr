using Zubr.Collections;
using Zubr.Diagnostics;
using Zubr.Parsing;
using Zubr.Syntax;
using Zubr.Text;

namespace Zubr;

public sealed class SyntaxTree
{
	private ConstArray<Diagnostic> _diagnostics;

	private readonly int[] _lineStartPositions;

	private CompilationUnitSyntax? _root;

	public SourceText Source { get; }

	public bool HasDiagnostics => !_diagnostics.IsDefaultOrEmpty;

	private SyntaxTree(SourceText source, int[] lineStartPositions)
	{
		Source = source;
		_lineStartPositions = lineStartPositions;
	}

	public CompilationUnitSyntax GetRoot()
	{
		return _root!;
	}

	public ConstArray<Diagnostic> GetDiagnostics()
	{
		return _diagnostics.SelfOrEmpty();
	}

	public Location GetLocation(int position)
	{
		int line = GetLine(position);

		if (line == -1)
		{
			return Location.None;
		}

		int column = Column(line, position);

		return new(Source.Path ?? string.Empty, line, column, LocationKind.Source);
	}

	public Location GetLocation(TextSpan span)
	{
		return GetLocation(span.Start);
	}

	public Location GetLocation(in Token token)
	{
		return GetLocation(token.Span);
	}

	public Location GetLocation(SyntaxNode node)
	{
		return GetLocation(node.Span);
	}

	public static SyntaxTree Parse(SourceText source)
	{
		SourceReader reader = source.GetSourceReader();
		Lexer lexer = new(reader);

		ArrayBuilder<Token> tokens = new(source.Length * 2);

		while (true)
		{
			Token token = lexer.NextToken();

			tokens.Add(token);

			if (token.IsKind(TokenKind.Eof))
			{
				break;
			}
		}

		DiagnosticBag diagnostics = new();

		SyntaxTree tree = new(source, lexer.GetLineStartPositions());

		if (lexer.HasDiagnostics)
		{
			CollectDiagnostics(lexer, tree, diagnostics);
		}

		SourceParser parser = new(tree, tokens.ToArray(), diagnostics);
		CompilationUnitSyntax root = parser.ParseCompilationUnit();
		tree.AttachRoot(root, diagnostics);

		return tree;
	}

	private int GetLine(int position)
	{
		int i = 1;

		while (i < _lineStartPositions.Length)
		{
			if (_lineStartPositions[i] >= position)
			{
				return i - 1;
			}

			i++;
		}

		return -1;
	}

	private int Column(int line, int absolutePosition)
	{
		int lineStart = _lineStartPositions[line];
		return absolutePosition - lineStart;
	}

	private void AttachRoot(CompilationUnitSyntax root, DiagnosticBag diagnostics)
	{
		_root = root;
		_diagnostics = diagnostics.ToConstArray();
	}

	private static void CollectDiagnostics(in Lexer lexer, SyntaxTree tree, DiagnosticBag diagnostics)
	{
		ArrayBuilder<InternalDiagnostic> errors = lexer.GetDiagnostics();

		for (int i = 0; i < errors.Length; i++)
		{
			InternalDiagnostic diag = errors[i];
			diagnostics.Push(diag.Code, tree.GetLocation(diag.Position));
		}
	}
}
