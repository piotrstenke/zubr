using Zubr.Text;

namespace Zubr.Diagnostics;

public sealed record DiagnosticMessage
{
	public string Code { get; }

	public string Message { get; }

	public DiagnosticSeverity Severity { get; }

	public Location Location { get; }

	public DiagnosticMessage(
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

	internal static DiagnosticMessage FromCode(ErrorCode code, Location location)
	{
		return new DiagnosticMessage(
			FormatCode(code),
			KnownDiagnostics.GetMessage(code),
			DiagnosticSeverity.Error,
			location
		);
	}

	internal static string FormatCode(ErrorCode code)
	{
		const string PREFIX = "ZR";

		int value = (int)code;

		if(value < 10)
		{
			return $"{PREFIX}000{value}";
		}

		if(value < 100)
		{
			return $"{PREFIX}00{value}";
		}

		if (value < 1000)
		{
			return $"{PREFIX}0{value}";
		}

		return $"{PREFIX}{(int)code}";
	}
}
