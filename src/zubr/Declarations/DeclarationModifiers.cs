using System;

namespace Zubr.Declarations;

[Flags]
internal enum DeclarationModifiers : uint
{
	None = 0,

	Open = 1 << 0,

	Final = 1 << 1,

	Limit = 1 << 2,

	Base = 1 << 3,

	Override = 1 << 4,

	Required = 1 << 5,

	Data = 1 << 6,

	Const = 1 << 7,

	Mutable = 1 << 8,

	Init = 1 << 9,

	Static = 1 << 10,

	Private = 1 << 11,

	Public = 1 << 12,

	Protected = 1 << 13,

	File = 1 << 14,

	Restricted = 1 << 15,

	Extern = 1 << 16,
}
