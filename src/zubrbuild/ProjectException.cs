using System;

namespace Zubr;

public class ProjectException : Exception
{
	public ProjectException()
	{
	}

	public ProjectException(string? message) : base(message)
	{
	}

	public ProjectException(string? message, Exception? innerException) : base(message, innerException)
	{
	}
}
