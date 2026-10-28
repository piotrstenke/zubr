using System.Diagnostics.CodeAnalysis;
using Zubr.Collections;
using Zubr.Diagnostics;

namespace Zubr.Emit;

public sealed class EmitResult
{
	private readonly ConstArray<Diagnostic> _diagnostics;

	[MemberNotNullWhen(true, nameof(Data))]
	public bool IsSuccess => Data is not null;

	public byte[]? Data { get; }

	public ConstArray<Diagnostic> Diagnostics => _diagnostics;

	[MemberNotNullWhen(false, nameof(Data))]
	public bool HasDiagnostics => !_diagnostics.IsEmpty;

	internal EmitResult(byte[]? data, ConstArray<Diagnostic> diagnostics)
	{
		Data = data;
		_diagnostics = diagnostics.SelfOrEmpty();
	}
}
