namespace Zubr.Diagnostics;

public sealed record Diagnostic
{
	public string Code { get; }

	public string Message { get; }

	public DiagnosticSeverity Severity { get; }

	public Diagnostic(string code, string message, DiagnosticSeverity severity)
	{
		Code = code;
		Message = message;
		Severity = severity;
	}

	public override string ToString()
	{
		return $"[{Severity}] {Code}: {Message}";
	}

	public override int GetHashCode()
	{
		return Code.GetHashCode();
	}

	public bool Equals(Diagnostic? other)
	{
		if(other is null)
		{
			return false;
		}

		return other.Code == Code;
	}
}
