namespace Zubr.Diagnostics;

internal enum ErrorCode
{
	Unknown,

	ERR_UnexpectedCharacter,

	ERR_UnexpectedToken,

	ERR_SyntaxError,

	ERR_InvalidModifier,

	ERR_DuplicateModifier,

	ERR_ElseIfNotSupported,

	ERR_TopUsesOutOfOrder,

	ERR_UnexpectedEndOfFile,

	ERR_UnexpectedGenericName,

	ERR_UnexpectedAliasedName,

	ERR_NonGenericConstraints
}
