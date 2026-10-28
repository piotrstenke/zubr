using System.Collections.Generic;
using Zubr.Text;

namespace Zubr.Diagnostics;

public readonly struct DiagnosticBag
{
	private readonly List<DiagnosticMessage> _diagnostics;

	public int Count => _diagnostics.Count;

	public DiagnosticBag()
	{
		_diagnostics = new();
	}

	internal DiagnosticBag(List<DiagnosticMessage> diagnostics)
	{
		_diagnostics = diagnostics;
	}

	internal DiagnosticBag(int capacity)
	{
		_diagnostics = new(capacity);
	}

	internal void Push(ErrorCode code, Location location)
	{
		_diagnostics.Add(DiagnosticMessage.FromCode(code, location));
	}

	public void Push(DiagnosticMessage diagnostic)
	{
		_diagnostics.Add(diagnostic);
	}

	public void Push(IEnumerable<DiagnosticMessage> diagnostics)
	{
		_diagnostics.AddRange(diagnostics);
	}

	public DiagnosticMessage[] GetDiagnostics()
	{
		return _diagnostics.ToArray();
	}
}
