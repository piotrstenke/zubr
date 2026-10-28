namespace Zubr;

public enum SyntaxKind
{
	None = 0,

	CompilationUnit = 1,

	// -----------------------------
	// Directives & Attributes
	// -----------------------------

	// use Module.Inner;
	// use Module.Inner as I;
	SimpleUseDirective,

	// use { X, Y, Z as Z1 } from Module;
	ListedUseDirective,

	// the '{ X, Y, Z as Z1 }' in 'use { X, Y, Z as Z1 } from Module;'
	UseDirectiveElementList,

	// the 'X', 'Y' in 'use { X, Y, Z as Z1 } from Module;'
	UseDirectiveElement,

	// [assembly: Value(2)]
	Attribute,

	// the 'assembly' in '[assembly: Value(2)]'
	AttributeTarget,

	// the '(2)' in '[assembly: Value(2)]'
	AttributeArgumentList,

	// the '2' in '[assembly: Value(2)]'
	AttributeArgument,

	// -----------------------------
	// Declarations
	// -----------------------------

	ModuleDeclaration,

	ClassDeclaration,

	StructDeclaration,

	TraitDeclaration,

	EnumDeclaration,

	EnumMemberDeclaration,

	// alias Test = LongTestName;
	TypeAliasDeclaration,

	AssociatedTypeDeclaration,

	FunctionDeclaration,

	ConstructorDeclaration,

	DestructorDeclaration,

	OperatorDeclaration,

	CastDeclaration,

	InvokerDeclaration,

	PropertyDeclaration,

	IndexerDeclaration,

	AccessorList,

	GetAccessor,

	SetAccessor,

	TypeParameterList,

	TypeParameter,

	TypeParameterInlineConstraintList,

	TypeParameterConstraintList,

	ClassConstraint,

	StructConstraint,

	EnumConstraint,

	TypeConstraint,

	BaseTypeList,

	SimpleBaseType,

	PrimaryBaseType,

	ParameterList,

	BracketParameterList,

	Parameter,

	// -----------------------------
	// Expressions
	// -----------------------------

	// "some text"
	StringLiteralExpression,

	// 1, 2L, 3u, 1.2f, 0.42e2 etc.
	NumericLiteralExpression,

	// true
	TrueLiteralExpression,

	// false
	FalseLiteralExpression,

	// null
	NullLiteralExpression,

	// a + b
	AddExpression,

	// a - b
	SubtractExpression,

	// a * b
	MultiplyExpression,

	// a / b
	DivideExpression,

	// a % b
	ModuloExpression,

	// a << b
	LeftShiftExpression,

	// a >> b
	RightShiftExpression,

	// a >>> b
	UnsignedRightShiftExpression,

	// a | b
	BitwiseOrExpression,

	// a & b
	BitwiseAndExpression,

	// a ^ b
	ExclusiveOrExpression,

	// a || b
	LogicalOrExpression,

	// a && b
	LogicalAndExpression,

	// a == b
	EqualsExpression,

	// a != b
	NotEqualsExpression,

	// a === b
	ReferenceEqualsExpression,

	// a !== b
	ReferenceNotEqualsExpression,

	// a < b
	LessThanExpression,

	// a <=
	LessThanOrEqualExpression,

	// a > b
	GreaterThanExpression,

	// a >= b
	GreaterThanOrEqualExpression,

	// +a
	UnaryPlusExpression,

	// -a
	UnaryMinusExpression,

	// ~a
	BitwiseNotExpression,

	// !a
	LogicalNotExpression,

	// *a
	PointerIndirectionExpression,

	// &b
	AddressOfExpression,

	// ++a
	PreIncrementExpression,

	// --a
	PreDecrementExpression,

	// a++
	PostIncrementExpression,

	// a--
	PostDecrementExpression,

	// a = b
	AssignmentExpression,

	// a += b
	AddAssignmentExpression,

	// a -= b
	SubtractAssignmentExpression,

	// a *= b
	MultiplyAssignmentExpression,

	// a /= b
	DivideAssignmentExpression,

	// a %= b
	ModuloAssignmentExpression,

	// a <<= b
	LeftShiftAssignmentExpression,

	// a >>= b
	RightShiftAssignmentExpression,

	// a >>>= b
	UnsignedRightShiftAssignmentExpression,

	// a |= b
	OrAssignmentExpression,

	// a &= b
	AndAssignmentExpression,

	// a ^= b
	ExclusiveOrAssignmentExpression,

	// a ??= b
	ConditionalAssignmentExpression,

	// (a + b)
	ParenthesizedExpression,

	// self
	SelfExpression,

	// base
	BaseExpression,

	// a..b, a..=b
	RangeExpression,

	// a.b
	SimpleMemberAccessExpression,

	// a->b
	PointerMemberAccessExpression,

	// array[0]
	ElementAccessExpression,

	// a?.b, a?[b], a?(b)
	ConditionalAccessExpression,

	// the '.b' in 'a?.b'
	MemberBindingExpression,

	// the '?[b]' in 'a?[b]
	ElementBindingExpression,

	// the '?(b)' in 'a?(b)'
	InvocationBindingExpression,

	// a ?? b
	CoalesceExpression,

	// the '(1, 2)' in 'a(1, 2)'
	ArgumentList,

	// the '1' and '2 'in 'a(1, 2)'
	Argument,

	// the '[0]' in 'array[0]'
	BracketArgumentList,

	// new { ... }, new(), new Test() { } etc.
	ObjectCreationExpression,

	// new int[] { }, new[5] etc.
	ArrayCreationExpression,

	// { "text", 1, new() { } }
	InitializerExpression,

	// a(1, 2)
	InvocationExpression,

	// base(1, 2)
	BaseConstructorInvocationExpression,

	// self(1, 2)
	SelfConstructorInvocationExpression,

	// x = match (y) { }
	MatchExpression,

	// the 'x => y' in match (x) { x => y }
	MatchExpressionBranch,

	// _
	DiscardExpression,

	// placeholder for expression when array type has no specified size (e.g. int[])
	SkippedArraySizeExpression,

	// -----------------------------
	// Types & Names
	// -----------------------------

	// Test
	IdentifierName,

	// Module.Class.Test
	QualifiedName,

	// Module::Class.Test
	AliasQualifiedName,

	// top::Class.Test or ::Class.Test
	TopQualifiedName,

	// Test<T>
	GenericName,

	// the empty in type argument in Test<>
	SkippedTypeArgument,

	// '<T>' in 'Test<T>'
	TypeArgumentList,

	// int, string, bool etc.
	PredefinedType,

	// int?
	NullableType,

	// float[], int[,] etc.
	ArrayType,

	// the '[,]' in 'int[,]' 
	ArrayRank,

	// int&
	ReferenceType,

	// int*
	PointerType,

	// -----------------------------
	// Statements
	// -----------------------------

	// { }
	Block,

	// ;
	EmptyStatement,

	// Expression followed by a ';'
	ExpressionStatement,

	// return x;
	ReturnStatement,

	// stop;
	StopStatement,

	// next;
	NextStatement,

	// mut x: int = 1;
	LocalDeclarationStatement,

	// jump DEFAULT;
	JumpStatement,

	// DEFAULT: ...;
	LabelStatement,

	// do { } while (condition);
	DoWhileStatement,

	// while(condition) { }
	WhileStatement,

	// for (x : collection) { }
	ForStatement,

	// if (x == y) { }
	IfStatement,

	// elif (x == y) { }
	ElifClause,

	// else { }
	ElseClause,

	// match (x) { }
	MatchStatement,

	// = value
	EqualsValueClause,

	// = int
	EqualsTypeClause,

	// => 1 + 1
	ArrowExpressionClause,

	// : string
	TypeSpecifier,

	// as x;
	AliasSpecifier,
}
