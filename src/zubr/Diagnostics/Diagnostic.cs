using System;
using Zubr.Text;

namespace Zubr.Diagnostics;

public sealed record Diagnostic
{
	public string Code { get; }

	public string Message { get; }

	public DiagnosticSeverity Severity { get; }

	public Location Location { get; }

	public Diagnostic(
		string code,
		string message,
		DiagnosticSeverity severity,
		Location location
	)
	{
		Code = code;
		Message = message;
		Severity = severity;
		Location = location;
	}

	public override string ToString()
	{
		return $"[{Severity}] {Code}: {Message} at {Location}";
	}

	public static Diagnostic Create(DiagnosticDefinition definition, Location location)
	{
		ArgumentNullException.ThrowIfNull(definition);

		return new(definition.Code, definition.Message, definition.Severity, location);
	}

	internal static Diagnostic Create(ErrorCode code, Location location)
	{
		return new Diagnostic(
			KnownDiagnostics.FormatCode(code),
			KnownDiagnostics.GetMessage(code),
			KnownDiagnostics.GetSeverity(code),
			location
		);
	}
}
