using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Globalization;
using System.Text;
using Zubr.Diagnostics;

namespace Zubr.Parsing;

internal struct Lexer
{
	private readonly SourceReader _reader;
	private readonly StringBuilder _builder;
	private List<InternalDiagnostic>? _errors;
	private readonly List<int> _lineStartPositions;

	private int _tokenStartPos;

	internal readonly bool HasErrors => _errors?.Count > 0;

	internal Lexer(SourceReader reader) : this(reader, null)
	{
	}

	internal Lexer(SourceReader reader, List<InternalDiagnostic>? diagnostics)
	{
		_reader = reader;
		_builder = new(32);
		_errors = diagnostics;
		_lineStartPositions = new(32)
		{
			0 // First line always start on position 1
		};
	}

	public Token NextToken()
	{
		ReadTrivia();
		return ReadToken();
	}

	internal readonly List<InternalDiagnostic>? GetErrors()
	{
		return _errors;
	}

	internal readonly int[] GetLineStartPositions()
	{
		return _lineStartPositions.ToArray();
	}

	private Token ReadToken()
	{
		_tokenStartPos = _reader.Position;

		char c = _reader.Peek();

		switch (c)
		{
			case '+':
				_reader.Move();

				if (_reader.Peek() == '+')
				{
					_reader.Move();

					return new(TokenKind.PlusPlus, "++", _tokenStartPos);
				}

				if (_reader.Peek() == '=')
				{
					_reader.Move();

					return new(TokenKind.PlusEquals, "+=", _tokenStartPos);
				}

				return new(TokenKind.Plus, "+", _tokenStartPos);

			case '-':
				_reader.Move();

				if (_reader.Peek() == '-')
				{
					_reader.Move();

					return new(TokenKind.MinusMinus, "--", _tokenStartPos);
				}

				if (_reader.Peek() == '=')
				{
					_reader.Move();

					return new(TokenKind.MinusEquals, "-=", _tokenStartPos);
				}

				if (_reader.Peek() == '>')
				{
					_reader.Move();

					return new(TokenKind.MinusGreaterThan, "->", _tokenStartPos);
				}

				return new(TokenKind.Minus, "-", _tokenStartPos);

			case '=':
				_reader.Move();

				if (_reader.Peek() == '=')
				{
					_reader.Move();

					if (_reader.Peek() == '=')
					{
						_reader.Move();

						return new(TokenKind.EqualsEqualsEquals, "===", _tokenStartPos);
					}

					return new(TokenKind.EqualsEquals, "==", _tokenStartPos);
				}

				if(_reader.Peek() == '>')
				{
					_reader.Move();
					return new(TokenKind.EqualsGreaterThan, "=>", _tokenStartPos);
				}

				return new(TokenKind.Equals, "=", _tokenStartPos);

			case '&':
				_reader.Move();

				if (_reader.Peek() == '&')
				{
					_reader.Move();

					return new(TokenKind.AmpersandAmpersand, "&&", _tokenStartPos);
				}

				if (_reader.Peek() == '=')
				{
					_reader.Move();

					return new(TokenKind.AmpersandEquals, "&=", _tokenStartPos);
				}

				return new(TokenKind.Ampersand, "&", _tokenStartPos);

			case '|':
				_reader.Move();

				if (_reader.Peek() == '|')
				{
					_reader.Move();

					return new(TokenKind.BarBar, "||", _tokenStartPos);
				}

				if (_reader.Peek() == '=')
				{
					_reader.Move();

					return new(TokenKind.BarEquals, "|=", _tokenStartPos);
				}

				return new(TokenKind.Bar, "|", _tokenStartPos);

			case '*':
				_reader.Move();

				if (_reader.Peek() == '*')
				{
					_reader.Move();

					if (_reader.Peek() == '=')
					{
						_reader.Move();
						return new(TokenKind.AsteriskAsteriskEquals, "**=", _tokenStartPos);
					}

					return new(TokenKind.AsteriskAsterisk, "**", _tokenStartPos);
				}

				if (_reader.Peek() == '=')
				{
					_reader.Move();

					return new(TokenKind.AsteriskEquals, "*=", _tokenStartPos);
				}

				return new(TokenKind.Asterisk, "*", _tokenStartPos);

			case '^':
				_reader.Move();

				if (_reader.Peek() == '=')
				{
					_reader.Move();

					return new(TokenKind.CaretEquals, "^=", _tokenStartPos);
				}

				return new(TokenKind.Caret, "^", _tokenStartPos);

			case '%':
				_reader.Move();

				if (_reader.Peek() == '=')
				{
					_reader.Move();

					return new(TokenKind.PercentEquals, "%=", _tokenStartPos);
				}

				return new(TokenKind.Percent, "%", _tokenStartPos);

			case '/':
				_reader.Move();

				if (_reader.Peek() == '=')
				{
					_reader.Move();

					return new(TokenKind.SlashEquals, "/=", _tokenStartPos);
				}

				return new(TokenKind.Slash, "/", _tokenStartPos);

			case '~':
				_reader.Move();
				return new(TokenKind.Tilde, "~", _tokenStartPos);

			case '(':
				_reader.Move();
				return new(TokenKind.OpenParen, "(", _tokenStartPos);

			case ')':
				_reader.Move();
				return new(TokenKind.CloseParen, ")", _tokenStartPos);

			case '[':
				_reader.Move();
				return new(TokenKind.OpenBracket, "[", _tokenStartPos);

			case ']':
				_reader.Move();
				return new(TokenKind.CloseBracket, "]", _tokenStartPos);

			case '{':
				_reader.Move();
				return new(TokenKind.OpenBrace, "{", _tokenStartPos);

			case '}':
				_reader.Move();
				return new(TokenKind.CloseBrace, "}", _tokenStartPos);

			case ',':
				_reader.Move();
				return new(TokenKind.Comma, ",", _tokenStartPos);

			case ';':
				_reader.Move();
				return new(TokenKind.Semicolon, ";", _tokenStartPos);

			case ':':
				_reader.Move();

				if (_reader.Peek() == ':')
				{
					_reader.Move();
					return new(TokenKind.ColonColon, "::", _tokenStartPos);
				}

				return new(TokenKind.Colon, ":", _tokenStartPos);

			case '!':
				_reader.Move();

				if (_reader.Peek() == '=')
				{
					_reader.Move();

					if (_reader.Peek() == '=')
					{
						_reader.Move();

						return new(TokenKind.ExclamationEqualsEquals, "!==", _tokenStartPos);
					}

					return new(TokenKind.ExclamationEquals, "!=", _tokenStartPos);
				}

				return new(TokenKind.Exclamation, "!", _tokenStartPos);

			case '>':
				_reader.Move();

				if (_reader.Peek() == '=')
				{
					_reader.Move();
					return new(TokenKind.GreaterThanEquals, ">=", _tokenStartPos);
				}

				if (_reader.Peek() == '>')
				{
					_reader.Move();

					if (_reader.Peek() == '=')
					{
						_reader.Move();
						return new(TokenKind.GreaterThanGreaterThanEquals, ">>=", _tokenStartPos);
					}

					return new(TokenKind.GreaterThanGreaterThan, ">>", _tokenStartPos);
				}

				return new(TokenKind.GreaterThan, ">", _tokenStartPos);

			case '<':
				_reader.Move();

				if (_reader.Peek() == '=')
				{
					_reader.Move();
					return new(TokenKind.LessThanEquals, "<=", _tokenStartPos);
				}

				if (_reader.Peek() == '<')
				{
					_reader.Move();

					if (_reader.Peek() == '=')
					{
						_reader.Move();
						return new(TokenKind.LessThanLessThanEquals, "<<=", _tokenStartPos);
					}

					return new(TokenKind.LessThanLessThan, "<<", _tokenStartPos);
				}

				return new(TokenKind.LessThan, "<", _tokenStartPos);

			case '?':
				_reader.Move();

				if (_reader.Peek() == '?')
				{
					_reader.Move();

					if (_reader.Peek() == '=')
					{
						_reader.Move();

						return new(TokenKind.QuestionQuestionEquals, "??=", _tokenStartPos);
					}

					return new(TokenKind.QuestionQuestion, "??", _tokenStartPos);
				}

				return new(TokenKind.Question, "?", _tokenStartPos);

			case '.':
				if (SyntaxFacts.IsDigit(_reader.Peek(1)))
				{
					return new(TokenKind.NumericLiteral, ReadNumericLiteral(out object? decimalValue), _tokenStartPos, decimalValue);
				}

				_reader.Move();

				if (_reader.Peek() == '.')
				{
					return new(TokenKind.DotDot, "..", _tokenStartPos);
				}

				return new(TokenKind.Dot, ".", _tokenStartPos);

			case '\'':
			case '\"':
				return new(TokenKind.StringLiteral, ReadStringLiteral(out string? stringValue), _tokenStartPos, stringValue);

			case >= '0' and <= '9':
				return new(TokenKind.NumericLiteral, ReadNumericLiteral(out object? numericValue), _tokenStartPos, numericValue);

			case '_':
			case (>= 'a' and <= 'z') or (>= 'A' and <= 'Z'):
				return ReadIdentifierOrKeyword();

			case SourceReader.InvalidChar:
				return new(TokenKind.Eof, string.Empty, _tokenStartPos);

			default:
				_reader.Move();
				AddError(ErrorCode.ERR_UnexpectedCharacter);
				return new(TokenKind.None, string.Empty, _tokenStartPos);
		}
	}

	private readonly void ReadTrivia()
	{
		char c;

		while ((c = _reader.Peek()) != SourceReader.InvalidChar)
		{
			if (IsNewLine(c))
			{
				_lineStartPositions.Add(_reader.Position + 1);
				_reader.Move();
				continue;
			}

			if (char.IsWhiteSpace(c))
			{
				_reader.Move();
				continue;
			}

			if (c == '/')
			{
				// Single-line comment.
				if (_reader.Peek(1) == '/')
				{
					_reader.Move(2);
					ReadUntilNewLine();

					continue;
				}
				else
				{
					// Not a comment.
					return;
				}
			}

			if (c == '*')
			{
				// Multi-line comment.
				if (_reader.Peek(1) == '*' && _reader.Peek(2) == '*')
				{
					_reader.Move(3);
					ReadUntilMultiLineCommentEnd();

					continue;
				}
				else
				{
					// Not a comment.
					return;
				}
			}

			// End of trivia.
			break;
		}
	}

	private readonly Token ReadIdentifierOrKeyword()
	{
		string identifier = ReadIdentifier();

		TokenKind keyword = SyntaxFacts.GetKeyword(identifier);

		if (keyword == TokenKind.None)
		{
			return new(TokenKind.Identifier, identifier, _tokenStartPos);
		}

		if (keyword == TokenKind.True)
		{
			return new(keyword, identifier, _tokenStartPos, true);
		}

		if (keyword == TokenKind.False)
		{
			return new(keyword, identifier, _tokenStartPos, false);
		}

		return new(keyword, identifier, _tokenStartPos);
	}

	private readonly string ReadIdentifier()
	{
		// Add the first char.
		char c = _reader.Read();
		_builder.Append(c);

		while ((c = _reader.Peek()) != SourceReader.InvalidChar)
		{
			if (IsValidIdentifierCharacter(c))
			{
				_builder.Append(c);
				_reader.Move();
			}
			else
			{
				// End of indentifier.
				break;
			}
		}

		return ToStringAndClear();
	}

	private string ReadStringLiteral(out string? value)
	{
		string literal = ReadRawStringLiteral();
		ReadOnlySpan<char> span = literal.AsSpan();

		// Ill-formed literal.
		if (span.Length == 0 || span.Length == 1 || span[^1] != '\"')
		{
			value = default;
			return literal;
		}

		// Empty string.
		if (span.Length == 2)
		{
			value = string.Empty;
			return literal;
		}

		// Skip start and end quotes.
		span = span[1..^1];

		for (int i = 0; i < span.Length; i++)
		{
			// New line character in string, invalid.
			if (IsNewLine(span[i]))
			{
				goto ERROR;
			}

			if (span[i] == '\\')
			{
				int next = i + 1;

				// Empty escape character, invalid.
				if (next > span.Length - 1)
				{
					goto ERROR;
				}

				char escaped = GetEscapedChar(span[next]);

				// Invalid escape character.
				if (escaped == SourceReader.InvalidChar)
				{
					goto ERROR;
				}

				_builder.Append(escaped);

				continue;
			}

			_builder.Append(span[i]);
		}

		value = ToStringAndClear();
		return literal;

	ERROR:
		AddError(ErrorCode.ERR_SyntaxError);
		_builder.Clear();
		value = null;
		return literal;
	}

	private string ReadRawStringLiteral()
	{
		char qoute = _reader.Read();

		// Append the quote.
		_builder.Append(qoute);

		while (true)
		{
			char c = _reader.Peek();

			// Eof
			if (c == SourceReader.InvalidChar)
			{
				AddError(ErrorCode.ERR_UnexpectedEndOfFile);
				break;
			}

			_reader.Move();

			_builder.Append(c);

			// Escaped quote, so don't end the literal.
			if (c == '\\' && _reader.Peek() == qoute)
			{
				_builder.Append(_reader.Read());
				continue;
			}

			// End of the literal.
			if (c == qoute)
			{
				break;
			}
		}

		return ToStringAndClear();
	}

	private string ReadNumericLiteral(out object? value)
	{
		bool hasPoint = false;
		bool allowUnderscore = false;

		NumberLiteralType type = NumberLiteralType.Int;

		while (true)
		{
			char c = _reader.Peek();

			// Eof
			if (c == SourceReader.InvalidChar)
			{
				break;
			}

			if (SyntaxFacts.IsDigit(c))
			{
				allowUnderscore = true;
				AppendAndMove(c);
				continue;
			}

			if (c == '.')
			{
				if (hasPoint)
				{
					// Ill-formed literal
					AddError(ErrorCode.ERR_SyntaxError);
					value = null;
					return ToStringAndClear();
				}

				allowUnderscore = false;
				hasPoint = true;
				AppendAndMove(c);
				type = NumberLiteralType.Double;
				continue;
			}

			if (c == '_')
			{
				if (!allowUnderscore)
				{
					// Ill-formed literal.
					AddError(ErrorCode.ERR_SyntaxError);
					value = null;
					return ToStringAndClear();
				}

				char next = _reader.Peek(1);

				if (next == SourceReader.InvalidChar)
				{
					break;
				}

				if (!SyntaxFacts.IsDigit(next))
				{
					// Ill-formed literal.
					AddError(ErrorCode.ERR_SyntaxError);
					value = null;
					return ToStringAndClear();
				}

				AppendAndMove(c);
				AppendAndMove(next);

				continue;
			}

			if (c == 'f' || c == 'F')
			{
				AppendAndMove(c);
				type = NumberLiteralType.Float;

				// End of the literal.
				break;
			}

			if (c == 'd' || c == 'D')
			{
				AppendAndMove(c);
				type = NumberLiteralType.Double;

				// End of the literal.
				break;
			}

			if (c == 'm' || c == 'M')
			{
				AppendAndMove(c);
				type = NumberLiteralType.Money;

				// End of the literal.
				break;
			}

			if (!hasPoint)
			{
				if (c == 'l' || c == 'L')
				{
					AppendAndMove(c);

					c = _reader.Peek();

					if (c == 'u' || c == 'U')
					{
						AppendAndMove(c);
						type = NumberLiteralType.ULong;
					}
					else
					{
						type = NumberLiteralType.Long;
					}

					// End of the literal.
					break;
				}
				else if (c == 'u' || c == 'U')
				{
					AppendAndMove(c);

					c = _reader.Peek();

					if (c == 'l' || c == 'L')
					{
						AppendAndMove(c);
						type = NumberLiteralType.Long;
					}
					else
					{
						type = NumberLiteralType.UInt;
					}

					// End of the literal.
					break;
				}
			}

			// End of the literal.
			break;
		}

		string literal = ToStringAndClear();
		TryParseNumericValue(literal, type, out value);

		return literal;
	}

	private bool TryParseNumericValue(string literal, NumberLiteralType type, out object? value)
	{
		switch (type)
		{
			case NumberLiteralType.Int:
				if (!int.TryParse(literal, NumberStyles.None, CultureInfo.InvariantCulture, out int intValue))
				{
					AddError(ErrorCode.ERR_SyntaxError);
					value = null;
					return false;
				}

				value = intValue;
				return true;

			case NumberLiteralType.Long:
				if (!long.TryParse(literal, NumberStyles.None, CultureInfo.InvariantCulture, out long longValue))
				{
					AddError(ErrorCode.ERR_SyntaxError);
					value = null;
					return false;
				}

				value = longValue;
				return true;

			case NumberLiteralType.UInt:
				if (!uint.TryParse(literal, NumberStyles.None, CultureInfo.InvariantCulture, out uint uintValue))
				{
					AddError(ErrorCode.ERR_SyntaxError);
					value = null;
					return false;
				}

				value = uintValue;
				return true;

			case NumberLiteralType.ULong:
				if (!ulong.TryParse(literal, NumberStyles.None, CultureInfo.InvariantCulture, out ulong ulongValue))
				{
					AddError(ErrorCode.ERR_SyntaxError);
					value = null;
					return false;
				}

				value = ulongValue;
				return true;

			case NumberLiteralType.Float:
				if (!float.TryParse(literal, NumberStyles.AllowDecimalPoint | NumberStyles.AllowExponent, CultureInfo.InvariantCulture, out float floatValue))
				{
					AddError(ErrorCode.ERR_SyntaxError);
					value = null;
					return false;
				}

				value = floatValue;
				return true;

			case NumberLiteralType.Double:
				if (!double.TryParse(literal, NumberStyles.AllowDecimalPoint | NumberStyles.AllowExponent, CultureInfo.InvariantCulture, out double doubleValue))
				{
					AddError(ErrorCode.ERR_SyntaxError);
					value = null;
					return false;
				}

				value = doubleValue;
				return true;

			case NumberLiteralType.Money:
				if (!decimal.TryParse(literal, NumberStyles.AllowDecimalPoint | NumberStyles.AllowExponent, CultureInfo.InvariantCulture, out decimal decimalValue))
				{
					AddError(ErrorCode.ERR_SyntaxError);
					value = null;
					return false;
				}

				value = decimalValue;
				return true;

			default:
				throw new UnreachableException();
		}
	}

	private readonly void ReadUntilNewLine()
	{
		char c;

		while ((c = _reader.Peek()) != SourceReader.InvalidChar)
		{
			_reader.Move();

			if(IsNewLine(c))
			{
				break;
			}
		}
	}

	private readonly void ReadUntilMultiLineCommentEnd()
	{
		char c;

		while ((c = _reader.Peek()) != SourceReader.InvalidChar)
		{
			_reader.Move();

			if(c == '*' && _reader.Peek() == '*')
			{
				_reader.Move();

				// End of the multi-line comment.
				if (_reader.Peek() == '*')
				{
					_reader.Move();
					return;
				}
			}
		}
	}

	private readonly string ToStringAndClear()
	{
		string str = _builder.ToString();
		_builder.Clear();
		return str;
	}

	private readonly void AppendAndMove(char c)
	{
		_builder.Append(c);
		_reader.Move();
	}

	private void AddError(ErrorCode code)
	{
		_errors ??= new();

		_errors.Add(new InternalDiagnostic(code, _tokenStartPos));
	}

	private static char GetEscapedChar(char c)
	{
		return c switch
		{
			'\\' or '"' or '\'' => c,
			'r' => '\u000d',
			'n' => '\u000a',
			't' => '\u0009',
			_ => SourceReader.InvalidChar, // Unknown escape character
		};
	}

	private static bool IsNewLine(char c)
	{
		return c == '\n';
	}

	private static bool IsValidIdentifierCharacter(char c)
	{
		return c == '_' || SyntaxFacts.IsDigit(c) || SyntaxFacts.IsAsciiLetter(c);
	}

	private enum NumberLiteralType : byte
	{
		Int,

		Long,

		UInt,

		ULong,

		Float,

		Double,

		Money
	}
}
