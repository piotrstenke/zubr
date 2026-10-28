using System;

namespace Zubr;

public readonly record struct ConfigurationValue
{
	private readonly string? _value;

	public bool IsEmpty => string.IsNullOrWhiteSpace(_value);

	internal ConfigurationValue(string value)
	{
		_value = value;
	}

	public int GetInt32()
	{
		if(int.TryParse(_value, out int result))
		{
			return result;
		}

		return 0;
	}

	public string GetString()
	{
		return _value ?? string.Empty;
	}

	public bool GetBoolean(bool defaultValue = false)
	{
		return _value?.Equals("true", StringComparison.OrdinalIgnoreCase) ?? defaultValue;
	}

	public T GetEnum<T>() where T : struct, Enum
	{
		if (!Enum.TryParse(_value, out T result))
		{
			return default;
		}

		return result;
	}

	public object GetEnum(Type enumType)
	{
		if (!Enum.TryParse(enumType, _value, out object? result))
		{
			return 0;
		}

		return result;
	}

	public override string ToString()
	{
		return GetString();
	}

	public override int GetHashCode()
	{
		return _value?.GetHashCode() ?? 0;
	}

	public bool Equals(ConfigurationValue other)
	{
		return other._value == _value;
	}
}
