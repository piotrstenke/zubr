using System;
using System.Diagnostics;

namespace Zubr.Text;

[DebuggerDisplay("{ToString(),nq}")]
public readonly record struct Location
{
	public static Location None => new(string.Empty, 0, 0, LocationKind.None);

	public bool IsDefault => this == default;

	public string Path { get; }

	public int Line { get; }

	public int Column { get; }

	public LocationKind Kind { get; }

	public Location(string path, int line, int column, LocationKind kind)
	{
		Path = path;
		Line = line;
		Column = column;
		Kind = kind;
	}

	public override string ToString()
	{
		return $"{Path}:{Line + 1},{Column + 1}";
	}

	public override int GetHashCode()
	{
		return HashCode.Combine(Line, Column, Path, Kind);
	}

	public bool Equals(Location other)
	{
		return
			other.Line == Line &&
			other.Column == Column &&
			other.Path == Path &&
			other.Kind == Kind;
	}
}
