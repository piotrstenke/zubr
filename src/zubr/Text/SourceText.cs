using System.IO;
using System.Text;
using Zubr.Parsing;

namespace Zubr.Text;

public sealed class SourceText
{
	private readonly string _source;

	public Encoding Encoding { get; }

	public string? Path { get; }

	public int Length => _source.Length;

	public char this[int index] => _source[index];

	private SourceText(string source, string? path, Encoding encoding)
	{
		_source = source;
		Path = path;
		Encoding = encoding;
	}

	public override string ToString()
	{
		return Path ?? "in-memory";
	}

	internal SourceReader GetSourceReader()
	{
		return new SourceReader(this);
	}

	public static SourceText FromFile(string path, Encoding? encoding = null)
	{
		encoding ??= Encoding.UTF8;

		string source = File.ReadAllText(path, encoding);
		return new(source, path, encoding);
	}

	public static SourceText FromSource(string source, Encoding? encoding = null)
	{
		return FromSource(source, null, encoding);
	}

	public static SourceText FromSource(string source, string? sourcePath, Encoding? encoding = null)
	{
		return new SourceText(source, sourcePath, encoding ?? Encoding.UTF8);
	}
}
