namespace Zubr.Symbols;

public enum FunctionKind
{
	Normal = 0,

	Constructor = 1,

	Destructor = 2,

	Operator = 3,

	Cast = 4,

	Invoker = 5,

	Local = 6,

	Getter = 7,

	Setter = 8,
}
