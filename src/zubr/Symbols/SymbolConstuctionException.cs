using System;
using Zubr.Binding;

namespace Zubr.Symbols;

public class SymbolConstuctionException : BindingException
{
	public SymbolConstuctionException()
	{
	}

	public SymbolConstuctionException(string? message) : base(message)
	{
	}

	public SymbolConstuctionException(string? message, Exception? innerException) : base(message, innerException)
	{
	}
}
