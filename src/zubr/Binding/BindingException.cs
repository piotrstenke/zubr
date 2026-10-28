using System;

namespace Zubr.Binding;

public class BindingException : CompilationException
{
	public BindingException()
	{
	}

	public BindingException(string? message) : base(message)
	{
	}

	public BindingException(string? message, Exception? innerException) : base(message, innerException)
	{
	}
}
