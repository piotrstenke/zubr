using System;

namespace Zubr;

public sealed class ConfigurationKeyNotFoundException : ProjectException
{
	public string Key { get; }

	public ConfigurationKeyNotFoundException(string key) : this(key, $"Key '{key}' was not found in the project configuration.")
	{
	}

	public ConfigurationKeyNotFoundException(string key, string? message) : base(message)
	{
		Key = key;
	}

	public ConfigurationKeyNotFoundException(string key, string? message, Exception? innerException) : base(message, innerException)
	{
		Key = key;
	}
}
