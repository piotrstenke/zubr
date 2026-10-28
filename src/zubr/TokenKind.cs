namespace Zubr;

public enum TokenKind
{
	// Not a token.
	None,

	// =
	Equals,

	// ==
	EqualsEquals,

	// ===
	EqualsEqualsEquals,

	// =>
	EqualsGreaterThan,

	// !
	Exclamation,

	// !=
	ExclamationEquals,

	// !==
	ExclamationEqualsEquals,

	// >
	GreaterThan,

	// >>
	GreaterThanGreaterThan,

	// >>>
	GreaterThanGreaterThanGreaterThan,

	// >=
	GreaterThanEquals,

	// >>=
	GreaterThanGreaterThanEquals,

	// >>>=
	GreaterThanGreaterThanGreaterThanEquals,

	// <
	LessThan,

	// <<
	LessThanLessThan,

	// <=
	LessThanEquals,

	// <<=
	LessThanLessThanEquals,

	// +
	Plus,

	// ++
	PlusPlus,

	// += 
	PlusEquals,

	// -
	Minus,

	// --
	MinusMinus,

	// -=
	MinusEquals,

	// ->
	MinusGreaterThan,

	// *
	Asterisk,

	// **
	AsteriskAsterisk,

	// *=
	AsteriskEquals,

	// **=
	AsteriskAsteriskEquals,

	// %
	Percent,

	// %=
	PercentEquals,

	// /
	Slash,

	// /=
	SlashEquals,

	// ^
	Caret,

	// ^=
	CaretEquals,

	// |
	Bar,

	// ||
	BarBar,

	// |=
	BarEquals,

	// &
	Ampersand,

	// &&
	AmpersandAmpersand,

	// &=
	AmpersandEquals,

	// ~
	Tilde,

	// (
	OpenParen,

	// )
	CloseParen,

	// {
	OpenBrace,

	// }
	CloseBrace,

	// [
	OpenBracket,

	// ]
	CloseBracket,

	// :
	Colon,

	// ::
	ColonColon,

	// ;
	Semicolon,

	// .
	Dot,

	// ..
	DotDot,

	// ,
	Comma,

	// ?
	Question,

	// ??
	QuestionQuestion,

	// ??=
	QuestionQuestionEquals,

	// Name of a variable, function, type etc.
	Identifier,

	// "abc"
	StringLiteral,

	// 1, 2L, 3u etc. 1.2f, 0.42e2 etc.
	NumericLiteral,

	//----------------------------------
	// KEYWORDS
	//----------------------------------

	// class
	Class = 100,

	Struct,

	Enum,

	Trait,

	Attr,

	Fn,

	Op,

	Cast,

	Type,

	Where,

	Module,

	Top,

	Alias,

	Use,

	As,

	From,

	False,

	True,

	Null,

	Void,

	If,

	Elif,

	Else,

	Match,

	For,

	Do,

	While,

	Jump,

	Next,

	Stop,

	Return,

	Priv,

	Pub,

	Prot,

	Rest,

	File,

	Mut,

	Const,

	Stat,

	Extern,

	Req,

	Data,

	Auto,

	Get,

	Set,

	Init,

	Value,

	Field,

	New,

	Free,

	Self,

	Base,

	Final,

	Over,

	Open,

	Limit,

	Test,

	Any,

	// An empty token that is valid but has no representation in the source.
	SkippedToken,

	// A token that was expected but is missing.
	MissingToken,

	// A token that is unexpected or invalid.
	BadToken,

	Eof = int.MaxValue
}
