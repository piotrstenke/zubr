namespace Zubr.Diagnostics;

public sealed record DiagnosticDefinition
{
	public string Code { get; }

	public string Message { get; }

	public DiagnosticSeverity Severity { get; }

	public DiagnosticDefinition(string code, string message, DiagnosticSeverity severity)
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

	public bool Equals(DiagnosticDefinition? other)
	{
		if(other is null)
		{
			return false;
		}

		return other.Code == Code;
	}
}
