using System.Collections.Generic;
using System.Linq;
using Zubr.Diagnostics;
using Zubr.Symbols;
using Zubr.Syntax;

namespace Zubr.Binding;

internal sealed class Binder
{
	private readonly DiagnosticBag _diagnostics;

	private readonly Scope _scope;

	internal Binder(Scope scope, DiagnosticBag diagnostics)
	{
		_scope = scope;
		_diagnostics = diagnostics;
	}

	public void Bind(SyntaxTree tree)
	{
		Bind([tree]);
	}

	public void Bind(IEnumerable<SyntaxTree> trees)
	{
		CompilationUnitSyntax[] units = trees
			.Select(x => x.GetRoot())
			.ToArray();

		HandleCompilationUnits(units);
	}

	private AssemblySymbol HandleCompilationUnits(CompilationUnitSyntax[] units)
	{
		ModuleSymbol top = CreateTopModule();

		foreach (CompilationUnitSyntax unit in units)
		{
			foreach (ModuleDeclarationSyntax module in unit.Members.OfType<ModuleDeclarationSyntax>())
			{
				//DeclareModule(module);
			}
		}

		AssemblySymbol assembly = CreateAssemblySymbol(top);
		return assembly;
	}

	private static AssemblySymbol CreateAssemblySymbol(ModuleSymbol top)
	{
		return new("assembly", top);
	}

	private static ModuleSymbol CreateTopModule()
	{
		return new(SyntaxFacts.GetText(TokenKind.Top));
	}

	private static ModuleSymbol CreateModule(string name)
	{
		return new(name);
	}

	//private void DeclareModule(ModuleDeclarationSyntax node, ModuleSymbol top)
	//{
	//	switch(node.Name)
	//	{
	//		// module top;
	//		case null:
	//			break;

	//		// module test;
	//		case IdentifierNameSyntax identifier:
	//			{
	//				string name = identifier.Identifier.Text;

	//				if(_scope.Lookup(name) is null)
	//				{
	//					_scope.AddSymbol(CreateModule(name));
	//				}
	//			}

	//			break;

	//		// module test.inner;
	//		case QualifiedNameSyntax qualified:
	//			{
	//				NameSyntax current = qualified.Left;


	//			}

	//			break;

	//		default:
	//			// TODO: Add diagnostics.
	//			throw new UnreachableException();
	//	}

	//	foreach (MemberDeclarationSyntax member in node.Members)
	//	{

	//	}
	//}

	//private void DeclareType(TypeDeclarationSyntax node)
	//{

	//}

	//private void DeclareModule()
	//{

	//}

	//private void HandleUseDirective(UseDirectiveElementSyntax node)
	//{

	//}
}
