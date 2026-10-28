using System.Diagnostics.CodeAnalysis;
using Zubr.Diagnostics;

namespace Zubr.Emit;

public sealed class EmitResult
{
	private Diagnostic[]? _diagnostics;

	[MemberNotNullWhen(true, nameof(Data))]
	public bool IsSuccess => Data is not null;

	public byte[]? Data { get; }

	public Diagnostic[] Diagnostics => _diagnostics ??= [];

	[MemberNotNullWhen(false, nameof(Data))]
	public bool HasDiagnostics => _diagnostics is not null;

	internal EmitResult(byte[]? data, Diagnostic[]? diagnostics)
	{
		Data = data;
		_diagnostics = diagnostics;
	}
}
