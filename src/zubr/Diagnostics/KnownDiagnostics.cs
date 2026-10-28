using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;

namespace Zubr.Diagnostics;

internal static class KnownDiagnostics
{
	private static readonly Dictionary<ErrorCode, string> _codes = new()
	{
		[ErrorCode.ERR_SyntaxError] = "Syntax error",
		[ErrorCode.ERR_UnexpectedCharacter] = "Unexpected character",
		[ErrorCode.ERR_UnexpectedToken] = "Unexpected token",
		[ErrorCode.ERR_UnexpectedEndOfFile] = "Unexpected end of file",
		[ErrorCode.ERR_InvalidModifier] = "Invalid modifier",
		[ErrorCode.ERR_DuplicateModifier] = "Duplicate modifier",
		[ErrorCode.ERR_ElseIfNotSupported] = "Else if is not supported, use 'elif' instead",
		[ErrorCode.ERR_TopUsesOutOfOrder] = "Top use directives must be defined before regular use directives",
		[ErrorCode.ERR_UnexpectedGenericName] = "Unexpected generic name",
		[ErrorCode.ERR_UnexpectedAliasedName] = "Unexpected aliased name",
		[ErrorCode.ERR_NonGenericConstraints] = "Constrains are not allowed on non-generic declarations"
	};

	public static string GetMessage(ErrorCode code)
	{
		return _codes[code];
	}

	public static bool TryGetMessage(ErrorCode code, [NotNullWhen(true)] out string? message)
	{
		return _codes.TryGetValue(code, out message);
	}

	public static DiagnosticSeverity GetSeverity(ErrorCode code)
	{
		string str = code.ToString();

		if(str.StartsWith("ERR_"))
		{
			return DiagnosticSeverity.Error;
		}

		if(str.StartsWith("WARN_"))
		{
			return DiagnosticSeverity.Warning;
		}

		return DiagnosticSeverity.Hidden;
	}

	public static string FormatCode(ErrorCode code)
	{
		const string PREFIX = "ZR";

		int value = (int)code;

		if (value < 10)
		{
			return $"{PREFIX}000{value}";
		}

		if (value < 100)
		{
			return $"{PREFIX}00{value}";
		}

		if (value < 1000)
		{
			return $"{PREFIX}0{value}";
		}

		return $"{PREFIX}{(int)code}";
	}
}
