using System;

namespace Zubr;

public static class SyntaxFacts
{
	private const TokenKind START_KIND = TokenKind.None;
	private const TokenKind END_KIND = TokenKind.Eof;

	private const TokenKind FIRST_KEYWORD = TokenKind.Class;
	private const TokenKind LAST_KEYWORD = TokenKind.Any;

	public static bool IsDigit(char c)
	{
		return c >= '0' && c <= '9';
	}

	public static bool IsAsciiLetter(char c)
	{
		return IsLowerAsciiLetter(c) || IsUpperAsciiLetter(c);
	}

	public static bool IsLowerAsciiLetter(char c)
	{
		return c >= 'a' && c <= 'z';
	}

	public static bool IsUpperAsciiLetter(char c)
	{
		return c >= 'A' && c <= 'Z';
	}

	public static string GetText(TokenKind kind)
	{
		return kind switch
		{
			TokenKind.Equals => "=",
			TokenKind.EqualsEquals => "==",
			TokenKind.EqualsEqualsEquals => "===",
			TokenKind.EqualsGreaterThan => "=>",
			TokenKind.Exclamation => "!",
			TokenKind.ExclamationEquals => "!=",
			TokenKind.ExclamationEqualsEquals => "!==",
			TokenKind.GreaterThan => ">",
			TokenKind.GreaterThanGreaterThan => ">>",
			TokenKind.GreaterThanGreaterThanGreaterThan => ">>>",
			TokenKind.GreaterThanEquals => ">=",
			TokenKind.GreaterThanGreaterThanEquals => ">>=",
			TokenKind.GreaterThanGreaterThanGreaterThanEquals => ">>>=",
			TokenKind.LessThan => "<",
			TokenKind.LessThanLessThan => "<<",
			TokenKind.LessThanEquals => "<=",
			TokenKind.LessThanLessThanEquals => "<<=",
			TokenKind.Plus => "+",
			TokenKind.PlusPlus => "++",
			TokenKind.PlusEquals => "+=",
			TokenKind.Minus => "-",
			TokenKind.MinusMinus => "--",
			TokenKind.MinusEquals => "-=",
			TokenKind.MinusGreaterThan => "->",
			TokenKind.Asterisk => "*",
			TokenKind.AsteriskAsterisk => "**",
			TokenKind.AsteriskEquals => "*=",
			TokenKind.AsteriskAsteriskEquals => "**=",
			TokenKind.Percent => "%",
			TokenKind.PercentEquals => "%=",
			TokenKind.Slash => "/",
			TokenKind.SlashEquals => "/=",
			TokenKind.Caret => "^",
			TokenKind.CaretEquals => "^=",
			TokenKind.Bar => "|",
			TokenKind.BarBar => "||",
			TokenKind.BarEquals => "|=",
			TokenKind.Ampersand => "&",
			TokenKind.AmpersandAmpersand => "&&",
			TokenKind.AmpersandEquals => "&=",
			TokenKind.Tilde => "~",
			TokenKind.OpenParen => "(",
			TokenKind.CloseParen => ")",
			TokenKind.OpenBrace => "{",
			TokenKind.CloseBrace => "}",
			TokenKind.OpenBracket => "[",
			TokenKind.CloseBracket => "]",
			TokenKind.Colon => ":",
			TokenKind.ColonColon => "::",
			TokenKind.Semicolon => ";",
			TokenKind.Dot => ".",
			TokenKind.Comma => ",",
			TokenKind.Question => "?",
			TokenKind.QuestionQuestion => "??",
			TokenKind.QuestionQuestionEquals => "??=",

			// Keywords
			TokenKind.Class => "class",
			TokenKind.Struct => "struct",
			TokenKind.Enum => "enum",
			TokenKind.Trait => "trait",
			TokenKind.Attr => "attr",
			TokenKind.Module => "module",
			TokenKind.Top => "top",
			TokenKind.Alias => "alias",
			TokenKind.Use => "use",
			TokenKind.As => "as",
			TokenKind.From => "from",
			TokenKind.False => "false",
			TokenKind.True => "true",
			TokenKind.Null => "null",
			TokenKind.If => "if",
			TokenKind.Elif => "elif",
			TokenKind.Else => "else",
			TokenKind.Match => "match",
			TokenKind.For => "for",
			TokenKind.Do => "do",
			TokenKind.While => "while",
			TokenKind.Jump => "jump",
			TokenKind.Next => "next",
			TokenKind.Stop => "stop",
			TokenKind.Return => "return",
			TokenKind.Pub => "pub",
			TokenKind.Priv => "priv",
			TokenKind.Prot => "prot",
			TokenKind.Rest => "rest",
			TokenKind.File => "file",
			TokenKind.Mut => "mut",
			TokenKind.Const => "const",
			TokenKind.Stat => "stat",
			TokenKind.Extern => "extern",
			TokenKind.Req => "req",
			TokenKind.Get => "get",
			TokenKind.Set => "set",
			TokenKind.Init => "init",
			TokenKind.Value => "value",
			TokenKind.New => "new",
			TokenKind.Free => "free",
			TokenKind.Self => "self",
			TokenKind.Base => "base",
			TokenKind.Final => "final",
			TokenKind.Over => "over",
			TokenKind.Open => "open",
			TokenKind.Limit => "limit",
			TokenKind.Test => "test",
			TokenKind.Any => "any",
			TokenKind.Void => "void",
			TokenKind.Fn => "fn",
			TokenKind.Type => "type",
			TokenKind.Where => "where",
			TokenKind.Field => "field",
			TokenKind.Op => "op",
			TokenKind.Cast => "cast",
			TokenKind.Auto => "auto",
			TokenKind.Data => "data",

			_ => string.Empty,
		};
	}

	public static TokenKind GetKeyword(string value)
	{
		return value switch
		{
			"class" => TokenKind.Class,
			"struct" => TokenKind.Struct,
			"enum" => TokenKind.Enum,
			"trait" => TokenKind.Trait,
			"attr" => TokenKind.Attr,
			"module" => TokenKind.Module,
			"top" => TokenKind.Top,
			"alias" => TokenKind.Alias,
			"use" => TokenKind.Use,
			"as" => TokenKind.As,
			"from" => TokenKind.From,
			"false" => TokenKind.False,
			"true" => TokenKind.True,
			"null" => TokenKind.Null,
			"if" => TokenKind.If,
			"elif" => TokenKind.Elif,
			"else" => TokenKind.Else,
			"match" => TokenKind.Match,
			"for" => TokenKind.For,
			"do" => TokenKind.Do,
			"while" => TokenKind.While,
			"jump" => TokenKind.Jump,
			"next" => TokenKind.Next,
			"stop" => TokenKind.Stop,
			"return" => TokenKind.Return,
			"pub" => TokenKind.Pub,
			"priv" => TokenKind.Priv,
			"prot" => TokenKind.Prot,
			"rest" => TokenKind.Rest,
			"file" => TokenKind.File,
			"mut" => TokenKind.Mut,
			"const" => TokenKind.Const,
			"stat" => TokenKind.Stat,
			"extern" => TokenKind.Extern,
			"req" => TokenKind.Req,
			"get" => TokenKind.Get,
			"set" => TokenKind.Set,
			"init" => TokenKind.Init,
			"value" => TokenKind.Value,
			"new" => TokenKind.New,
			"free" => TokenKind.Free,
			"self" => TokenKind.Self,
			"base" => TokenKind.Base,
			"final" => TokenKind.Final,
			"over" => TokenKind.Over,
			"open" => TokenKind.Open,
			"limit" => TokenKind.Limit,
			"test" => TokenKind.Test,
			"any" => TokenKind.Any,
			"void" => TokenKind.Void,
			"fn" => TokenKind.Fn,
			"type" => TokenKind.Type,
			"where" => TokenKind.Where,
			"field" => TokenKind.Field,
			"op" => TokenKind.Op,
			"cast" => TokenKind.Cast,
			"auto" => TokenKind.Auto,
			"data" => TokenKind.Data,
			_ => TokenKind.None
		};
	}

	public static bool IsKeyword(string value)
	{
		return GetKeyword(value) != TokenKind.None;
	}

	public static bool IsKeyword(TokenKind kind)
	{
		return IsBetween(kind, FIRST_KEYWORD, LAST_KEYWORD);
	}

	public static bool IsKeyword(this in Token token)
	{
		return IsKeyword(token.Kind);
	}

	public static TokenKind GetContextualKind(this in Token token)
	{
		if (!token.IsKind(TokenKind.Identifier))
		{
			return token.Kind;
		}

		TokenKind kind = GetKeyword(token.Text);

		if (kind == TokenKind.None)
		{
			return token.Kind;
		}

		return kind;
	}

	public static bool IsContextualKeyword(this in Token token)
	{
		if (!token.IsKind(TokenKind.Identifier))
		{
			return false;
		}

		return GetKeyword(token.Text) != TokenKind.None;
	}

	public static bool IsContextualKeyword(TokenKind value)
	{
		return
			value == TokenKind.Free ||
			value == TokenKind.Get ||
			value == TokenKind.Set ||
			value == TokenKind.Init ||
			value == TokenKind.Value ||
			value == TokenKind.File ||
			value == TokenKind.Over ||
			value == TokenKind.Open ||
			value == TokenKind.Limit ||
			value == TokenKind.Final;
	}

	public static bool IsLiteral(TokenKind kind)
	{
		return kind == TokenKind.NumericLiteral || kind == TokenKind.StringLiteral;
	}

	public static bool IsLiteralExpression(SyntaxKind value)
	{
		return
			value == SyntaxKind.TrueLiteralExpression ||
			value == SyntaxKind.FalseLiteralExpression ||
			value == SyntaxKind.StringLiteralExpression ||
			//value == SyntaxKind.CharLiteralExpression ||
			value == SyntaxKind.NumericLiteralExpression;
	}

	public static SyntaxKind GetPrefixUnaryExpressionKind(TokenKind value)
	{
		return value switch
		{
			TokenKind.Plus => SyntaxKind.UnaryPlusExpression,
			TokenKind.PlusPlus => SyntaxKind.PreIncrementExpression,
			TokenKind.Minus => SyntaxKind.UnaryMinusExpression,
			TokenKind.MinusMinus => SyntaxKind.PreDecrementExpression,
			TokenKind.Exclamation => SyntaxKind.LogicalNotExpression,
			TokenKind.Tilde => SyntaxKind.BitwiseNotExpression,
			TokenKind.Ampersand => SyntaxKind.AddressOfExpression,
			TokenKind.Asterisk => SyntaxKind.PointerIndirectionExpression,
			_ => default
		};
	}

	public static SyntaxKind GetPostfixUnaryExpressionKind(TokenKind value)
	{
		return value switch
		{
			TokenKind.PlusPlus => SyntaxKind.PostIncrementExpression,
			TokenKind.MinusMinus => SyntaxKind.PostDecrementExpression,
			_ => default
		};
	}

	public static SyntaxKind GetBinaryExpressionKind(TokenKind value)
	{
		return value switch
		{
			TokenKind.Plus => SyntaxKind.AddExpression,
			TokenKind.Minus => SyntaxKind.SubtractExpression,
			TokenKind.Asterisk => SyntaxKind.MultiplyExpression,
			TokenKind.Slash => SyntaxKind.DivideExpression,
			TokenKind.Percent => SyntaxKind.ModuloExpression,
			TokenKind.Caret => SyntaxKind.ExclusiveOrExpression,
			TokenKind.Bar => SyntaxKind.BitwiseOrExpression,
			TokenKind.Ampersand => SyntaxKind.BitwiseAndExpression,
			TokenKind.GreaterThanGreaterThan => SyntaxKind.RightShiftExpression,
			TokenKind.LessThanLessThan => SyntaxKind.LeftShiftExpression,
			TokenKind.GreaterThanGreaterThanGreaterThan => SyntaxKind.UnsignedRightShiftExpression,
			TokenKind.EqualsEquals => SyntaxKind.EqualsExpression,
			TokenKind.EqualsEqualsEquals => SyntaxKind.ReferenceEqualsExpression,
			TokenKind.ExclamationEquals => SyntaxKind.NotEqualsExpression,
			TokenKind.ExclamationEqualsEquals => SyntaxKind.ReferenceNotEqualsExpression,
			TokenKind.GreaterThan => SyntaxKind.GreaterThanExpression,
			TokenKind.GreaterThanEquals => SyntaxKind.GreaterThanOrEqualExpression,
			TokenKind.LessThan => SyntaxKind.LessThanExpression,
			TokenKind.LessThanEquals => SyntaxKind.LessThanOrEqualExpression,
			TokenKind.BarBar => SyntaxKind.LogicalOrExpression,
			TokenKind.AmpersandAmpersand => SyntaxKind.LogicalAndExpression,
			TokenKind.DotDot => SyntaxKind.RangeExpression,
			TokenKind.QuestionQuestion => SyntaxKind.CoalesceExpression,

			// Assignment

			TokenKind.Equals => SyntaxKind.AssignmentExpression,
			TokenKind.PlusEquals => SyntaxKind.AddAssignmentExpression,
			TokenKind.MinusEquals => SyntaxKind.SubtractAssignmentExpression,
			TokenKind.AsteriskEquals => SyntaxKind.MultiplyAssignmentExpression,
			TokenKind.SlashEquals => SyntaxKind.DivideAssignmentExpression,
			TokenKind.PercentEquals => SyntaxKind.ModuloAssignmentExpression,
			TokenKind.CaretEquals => SyntaxKind.ExclusiveOrAssignmentExpression,
			TokenKind.BarEquals => SyntaxKind.BitwiseOrExpression,
			TokenKind.AmpersandEquals => SyntaxKind.BitwiseAndExpression,
			TokenKind.LessThanLessThanEquals => SyntaxKind.LeftShiftAssignmentExpression,
			TokenKind.GreaterThanGreaterThanEquals => SyntaxKind.RightShiftAssignmentExpression,
			TokenKind.GreaterThanGreaterThanGreaterThanEquals => SyntaxKind.UnsignedRightShiftAssignmentExpression,
			TokenKind.QuestionQuestionEquals => SyntaxKind.ConditionalAssignmentExpression,

			_ => default
		};
	}

	public static SyntaxKind GetLiteralExpressionKind(TokenKind value)
	{
		return value switch
		{
			TokenKind.StringLiteral => SyntaxKind.StringLiteralExpression,
			TokenKind.NumericLiteral => SyntaxKind.NumericLiteralExpression,
			//TokenKind.CharLiteral => SyntaxKind.CharLiteralExpression,
			TokenKind.True => SyntaxKind.TrueLiteralExpression,
			TokenKind.False => SyntaxKind.FalseLiteralExpression,
			_ => default
		};
	}

	public static SyntaxKind GetExpressionKind(TokenKind value)
	{
		SyntaxKind kind = GetBinaryExpressionKind(value);

		if (kind != default)
		{
			return kind;
		}

		kind = GetPostfixUnaryExpressionKind(value);

		if (kind != default)
		{
			return kind;
		}

		kind = GetPrefixUnaryExpressionKind(value);

		if (kind != default)
		{
			return kind;
		}

		kind = GetLiteralExpressionKind(value);

		return kind;
	}

	public static bool IsOverloadableOperator(this Token token)
	{
		return IsOverloadableOperator(token.Kind);
	}

	public static bool IsOverloadableOperator(TokenKind value)
	{
		if (IsComparisonOperator(value))
		{
			return true;
		}

		if (GetPrefixUnaryExpressionKind(value) != default)
		{
			return true;
		}

		return value is
			TokenKind.Asterisk or
			TokenKind.Percent or
			TokenKind.Slash or
			TokenKind.Ampersand or
			TokenKind.Bar or
			TokenKind.Caret or
			TokenKind.LessThanLessThan or
			TokenKind.GreaterThanGreaterThan or
			TokenKind.GreaterThanGreaterThanGreaterThan or
			TokenKind.False or
			TokenKind.True;
	}

	public static bool IsComparisonOperator(this in Token token)
	{
		return IsComparisonOperator(token.Kind);
	}

	public static bool IsComparisonOperator(TokenKind value)
	{
		return value is
			TokenKind.EqualsEquals or
			TokenKind.EqualsEqualsEquals or
			TokenKind.ExclamationEquals or
			TokenKind.ExclamationEqualsEquals or
			TokenKind.GreaterThan or
			TokenKind.GreaterThanEquals or
			TokenKind.LessThan or
			TokenKind.LessThanEquals;
	}

	public static bool IsAssignmentOperator(this in Token token)
	{
		return IsAssignmentOperator(token.Kind);
	}

	public static bool IsAssignmentOperator(TokenKind value)
	{
		return value is
			TokenKind.Equals or
			TokenKind.PlusEquals or
			TokenKind.MinusEquals or
			TokenKind.AsteriskEquals or
			TokenKind.SlashEquals or
			TokenKind.PercentEquals or
			TokenKind.BarEquals or
			TokenKind.CaretEquals or
			TokenKind.AmpersandEquals or
			TokenKind.LessThanLessThanEquals or
			TokenKind.GreaterThanGreaterThanEquals or
			TokenKind.GreaterThanGreaterThanGreaterThanEquals or
			TokenKind.QuestionQuestionEquals;
	}

	public static SyntaxKind GetAccessorKind(this in Token token)
	{
		return GetAccessorKind(token.Kind);
	}

	public static SyntaxKind GetAccessorKind(TokenKind value)
	{
		return value switch
		{
			TokenKind.Get => SyntaxKind.GetAccessor,
			TokenKind.Set => SyntaxKind.SetAccessor,
			_ => default
		};
	}

	public static bool IsAccessor(this in Token token)
	{
		return IsAccessor(token.Kind);
	}

	public static bool IsAccessor(TokenKind value)
	{
		return GetAccessorKind(value) != default;
	}

	public static bool IsAccessModifier(this in Token token)
	{
		return IsAccessModifier(token.Kind);
	}

	public static bool IsAccessModifier(TokenKind value)
	{
		return
			value == TokenKind.Pub ||
			value == TokenKind.Priv ||
			value == TokenKind.Prot ||
			value == TokenKind.Rest ||
			value == TokenKind.File;
	}

	public static bool IsModifier(this in Token token)
	{
		return IsModifier(token.Kind);
	}

	public static bool IsModifier(TokenKind value)
	{
		if (IsAccessModifier(value))
		{
			return true;
		}

		return value is
			TokenKind.Open or
			TokenKind.Mut or
			TokenKind.Init or
			TokenKind.Final or
			TokenKind.Const or
			TokenKind.Base or
			TokenKind.Limit or
			TokenKind.Over or
			TokenKind.Req or
			TokenKind.Stat or
			TokenKind.Auto or
			TokenKind.Attr or
			TokenKind.Data or
			TokenKind.Extern;
	}

	public static bool IsPredefinedType(this in Token token)
	{
		return IsPredefinedType(token.Kind);
	}

	public static bool IsPredefinedType(TokenKind value)
	{
		return value is
			TokenKind.Void or
			//TokenKind.BoolKeyword or
			//TokenKind.IntKeyword or
			//TokenKind.UIntKeyword or
			//TokenKind.ShortKeyword or
			//TokenKind.UShortKeyword or
			//TokenKind.LongKeyword or
			//TokenKind.ULongKeyword or
			//TokenKind.ByteKeyword or
			//TokenKind.SByteKeyword or
			//TokenKind.StringKeyword or
			//TokenKind.CharKeyword or
			//TokenKind.FloatKeyword or
			//TokenKind.DoubleKeyword or
			//TokenKind.HalfKeyword or
			//TokenKind.DecimalKeyword or
			//TokenKind.NIntKeyword or
			//TokenKind.NUIntKeyword or
			TokenKind.Any;
	}

	public static bool IsLiteral(this in Token token)
	{
		return IsLiteral(token.Kind);
	}

	public static bool IsValid(this in Token token)
	{
		return IsValid(token.Kind);
	}

	public static bool IsValid(TokenKind value)
	{
		return IsBetween(value, START_KIND, END_KIND) && !IsError(value);
	}

	public static bool IsValidIdentifier(string value)
	{
		ReadOnlySpan<char> span = value;

		if(span.Length == 0)
		{
			return false;
		}

		if (span[0] != '_' && !char.IsAsciiLetter(span[0]))
		{
			return false;
		}

		for (int i = 1; i < span.Length; i++)
		{
			if (span[i] != '_' && !char.IsAsciiLetterOrDigit(span[i]))
			{
				return false;
			}
		}

		if(IsKeyword(value))
		{
			return false;
		}

		return true;
	}

	public static bool IsError(this in Token token)
	{
		return IsError(token.Kind);
	}

	public static bool IsError(TokenKind value)
	{
		return value == TokenKind.BadToken || value == TokenKind.MissingToken;
	}

	private static bool IsBetween(TokenKind value, TokenKind start, TokenKind end)
	{
		return value >= start && value <= end;
	}
}
