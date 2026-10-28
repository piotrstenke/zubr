using System.Collections.Generic;
using Zubr.Collections;
using Zubr.Text;

namespace Zubr.Diagnostics;

public struct DiagnosticBag
{
	private ArrayBuilder<Diagnostic> _diagnostics;

	public readonly int Count => _diagnostics.Length;

	public readonly bool IsEmpty => Count == 0;

	public DiagnosticBag()
	{
		_diagnostics = default;
	}

	internal DiagnosticBag(int capacity)
	{
		_diagnostics = new(capacity);
	}

	internal void Push(ErrorCode code, Location location)
	{
		Push(Diagnostic.Create(code, location));
	}

	public void Push(Diagnostic diagnostic)
	{
		_diagnostics.Init();

		_diagnostics.Add(diagnostic);
	}

	public void Push(ConstArray<Diagnostic> diagnostics)
	{
		_diagnostics.Init(diagnostics.Length);

		_diagnostics.AddRange(diagnostics);
	}

	public void Push(IEnumerable<Diagnostic> diagnostics)
	{
		_diagnostics.Init();

		_diagnostics.AddRange(diagnostics);
	}

	public Diagnostic[] ToArray()
	{
		return _diagnostics.ToArray();
	}

	public ConstArray<Diagnostic> ToConstArray()
	{
		return _diagnostics.ToConstArray();
	}
}
