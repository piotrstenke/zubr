using System.Diagnostics.CodeAnalysis;

namespace Zubr.Load;

public sealed class ProjectLoadResult
{
	private readonly ErrorMessage[]? _errors;

	public ProjectSettings? Settings { get; }

	public string FilePath { get; }

	[MemberNotNullWhen(true, nameof(Settings))]
	public bool IsSuccess => Settings is not null;

	internal ProjectLoadResult(ProjectSettings? settings, string filePath, ErrorMessage[]? errors)
	{
		Settings = settings;
		FilePath = filePath;
		_errors = errors;
	}

	public ErrorMessage[] GetErrors()
	{
		if(_errors is null || _errors.Length == 0)
		{
			return [];
		}

		ErrorMessage[] errors = new ErrorMessage[_errors.Length];
		_errors.CopyTo(errors, 0);
		return errors;
	}
}
