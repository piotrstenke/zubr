using System;
using System.Diagnostics;
using Zubr.Text;

namespace Zubr;

[DebuggerDisplay("{Text ?? string.Empty,nq}")]
public readonly record struct Token
{
	public object? Value { get; }

	public string Text { get; }

	public int Position { get; }

	public TokenKind Kind { get; }

	public int Length => Text.Length;

	public bool IsNone => Kind == TokenKind.None;

	public bool Exists => !IsNone;

	public bool IsEmpty => Text.Length == 0;

	public TextSpan Span => new(Position, Length);

	internal Token(TokenKind kind, string text, int position)
	{
		Kind = kind;
		Text = text;
		Position = position;
	}

	internal Token(TokenKind kind, string text, int position, object? value)
	{
		Value = value;
		Kind = kind;
		Text = text;
		Position = position;
	}

	public override string ToString()
	{
		return Text ?? string.Empty;
	}

	public override int GetHashCode()
	{
		return HashCode.Combine(Text, Position, Kind);
	}

	public bool Equals(Token other)
	{
		return
			other.Text == Text &&
			other.Position == Position &&
			other.Kind == Kind;
	}

	public bool IsKind(TokenKind kind)
	{
		return Kind == kind;
	}
}
