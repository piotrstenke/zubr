using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using Zubr.Diagnostics;
using Zubr.Parsing;
using Zubr.Syntax;
using Zubr.Text;

namespace Zubr;

public sealed class SyntaxTree
{
	private DiagnosticMessage[]? _diagnostics;

	private readonly int[] _lineStartPositions;

	private CompilationUnitSyntax? _root;

	public SourceText Source { get; }

	public bool HasDiagnostics => _diagnostics is not null && _diagnostics.Length > 0;

	private SyntaxTree(SourceText source, int[] lineStartPositions)
	{
		Source = source;
		_lineStartPositions = lineStartPositions;
	}

	public CompilationUnitSyntax GetRoot()
	{
		return _root!;
	}

	public DiagnosticMessage[] GetDiagnostics()
	{
		return _diagnostics ?? Array.Empty<DiagnosticMessage>();
	}

	public Location GetLocation(int position)
	{
		return GetLocation(new TextSpan(position, position));
	}

	public Location GetLocation(TextSpan span)
	{
		int line = GetLine(span.Start);

		if (line == -1)
		{
			return Location.None;
		}

		int column = Column(line, span.Start);

		return new(Source.Path ?? string.Empty, line, column, LocationKind.Source);
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

		List<Token> tokens = new(source.Length * 2);

		while (true)
		{
			Token token = lexer.NextToken();

			tokens.Add(token);

			if (token.IsKind(TokenKind.Eof))
			{
				break;
			}
		}

		List<InternalDiagnostic>? diagnostics = lexer.GetErrors();
		SyntaxTree tree = new(source, lexer.GetLineStartPositions());

		SourceParser parser = new(tree, tokens.ToArray(), diagnostics);
		CompilationUnitSyntax root = parser.ParseCompilationUnit();

		diagnostics = parser.GetDiagnostics();

		tree.AttachRoot(root, GetDiagnostics(diagnostics, tree));
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

	private void AttachRoot(
		CompilationUnitSyntax root,
		DiagnosticMessage[]? diagnostics
	)
	{
		_root = root;
		_diagnostics = diagnostics;
	}

	[return: NotNullIfNotNull(nameof(diagnostics))]
	private static DiagnosticMessage[]? GetDiagnostics(List<InternalDiagnostic>? diagnostics, SyntaxTree tree)
	{
		if (diagnostics is null)
		{
			return null;
		}

		DiagnosticMessage[] array = new DiagnosticMessage[diagnostics.Count];

		for (int i = 0; i < diagnostics.Count; i++)
		{
			InternalDiagnostic diag = diagnostics[i];
			array[i] = DiagnosticMessage.FromCode(diag.Code, tree.GetLocation(diag.Position));
		}

		return array;
	}
}
