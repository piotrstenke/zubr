using System;
using System.Collections.Generic;
using System.Linq;
using Zubr.Collections;
using Zubr.Declarations;
using Zubr.Diagnostics;
using Zubr.Emit;

namespace Zubr;

public sealed class Compilation
{
	private ConstArray<Diagnostic> _diagnostics;

	public IEnumerable<SyntaxTree> SyntaxTrees { get; }

	public IEnumerable<MetadataReference> References { get; }

	public CompilationOptions Options { get; }

	public string AssemblyName { get; }

	public bool HasDiagnostics
	{
		get
		{
			if (!_diagnostics.IsDefaultOrEmpty)
			{
				return true;
			}

			FetchDiagnostics();
			return !_diagnostics.IsEmpty;
		}
	}

	public bool HasErrors
	{
		get
		{
			if (!_diagnostics.IsDefaultOrEmpty)
			{
				return HasErrorDiagnostics();
			}

			FetchDiagnostics();
			return HasErrorDiagnostics();
		}
	}

	internal Compilation(
		string assemblyName,
		CompilationOptions options,
		IEnumerable<SyntaxTree> syntaxTrees,
		IEnumerable<MetadataReference> references
	)
	{
		AssemblyName = assemblyName;
		SyntaxTrees = syntaxTrees;
		References = references;
		Options = options;
	}

	public ConstArray<Diagnostic> GetDiagnostics()
	{
		// The diagnostics were already fetched.
		if(!_diagnostics.IsDefault)
		{
			return _diagnostics;
		}

		FetchDiagnostics();
		return _diagnostics;
	}

	public EmitResult Emit()
	{
		return new([0], _diagnostics);
	}

	public CompilationReference CreateReference()
	{
		return new(this, new());
	}

	public Builder AddSyntaxTrees(SyntaxTree tree)
	{
		return CreateBuilder().AddSyntaxTrees(tree);
	}

	public Builder AddSyntaxTrees(IEnumerable<SyntaxTree> trees)
	{
		return CreateBuilder().AddSyntaxTrees(trees);
	}

	public Builder WithSyntaxTrees(SyntaxTree tree)
	{
		return CreateBuilder().WithSyntaxTrees(tree);
	}

	public Builder WithSyntaxTrees(IEnumerable<SyntaxTree> trees)
	{
		return CreateBuilder().WithSyntaxTrees(trees);
	}

	public Builder AddReferences(MetadataReference reference)
	{
		return CreateBuilder().AddReferences(reference);
	}

	public Builder AddReferences(IEnumerable<MetadataReference> references)
	{
		return CreateBuilder().AddReferences(references);
	}

	public Builder WithReferences(MetadataReference reference)
	{
		return CreateBuilder().WithReferences(reference);
	}

	public Builder WithReferences(IEnumerable<MetadataReference> references)
	{
		return CreateBuilder().WithReferences(references);
	}

	public Builder WithAssemblyName(string assemblyName)
	{
		return CreateBuilder().WithAssemblyName(assemblyName);
	}

	public Builder WithOptions(CompilationOptions options)
	{
		return CreateBuilder().WithOptions(options);
	}

	public static Builder Create(string assemblyName)
	{
		ArgumentException.ThrowIfNullOrWhiteSpace(assemblyName);
		return new(assemblyName);
	}

	public static Compilation Create(
		string assemblyName,
		IEnumerable<SyntaxTree> syntaxTrees,
		IEnumerable<MetadataReference> references,
		CompilationOptions? options = null
	)
	{
		ArgumentException.ThrowIfNullOrWhiteSpace(assemblyName);
		ArgumentNullException.ThrowIfNull(syntaxTrees);
		ArgumentNullException.ThrowIfNull(references);

		return new(assemblyName, options ?? CompilationOptions.Default, syntaxTrees, references);
	}

	private Builder CreateBuilder()
	{
		return new(this);
	}

	private bool HasErrorDiagnostics()
	{
		return _diagnostics.Any(x => x.Severity == DiagnosticSeverity.Error);
	}

	private void FetchDiagnostics()
	{
		DiagnosticBag diagnostics = new();

		FetchSyntaxTreeDiagnostics(diagnostics);

		AnalyzeTrees(diagnostics);

		if (!diagnostics.IsEmpty)
		{
			return;
		}

		_diagnostics = diagnostics.ToConstArray();

		void FetchSyntaxTreeDiagnostics(DiagnosticBag diagnostics)
		{
			foreach (SyntaxTree tree in SyntaxTrees)
			{
				if(!tree.HasDiagnostics)
				{
					continue;
				}

				diagnostics.Push(tree.GetDiagnostics());
			}
		}
	}

	private void AnalyzeTrees(DiagnosticBag diagnostics)
	{
		foreach (SyntaxTree tree in SyntaxTrees)
		{
			ConstArray<Diagnostic> array = Analyze(tree);

			if(!array.IsDefaultOrEmpty)
			{
				diagnostics.Push(array);
			}
		}
	}

	private static ConstArray<Diagnostic> Analyze(SyntaxTree tree)
	{
		RootSingleModuleDeclaration root = DeclarationTreeBuilder.Build(tree);
		return root.Diagnostics;
	}

	public sealed class Builder : IBuilder<Compilation>
	{
		private List<SyntaxTree>? _trees;

		private List<MetadataReference>? _references;

		private CompilationOptions? _options;

		private string _assemblyName;

		internal Builder(string assemblyName)
		{
			_assemblyName = assemblyName;
		}

		internal Builder(Compilation original)
		{
			_assemblyName = original.AssemblyName;
			_trees = original.SyntaxTrees.ToList();
			_references = original.References.ToList();
			_options = original.Options with { };
		}

		public Builder AddSyntaxTrees(SyntaxTree tree)
		{
			CollectionHelpers.Add(ref _trees, tree);
			return this;
		}

		public Builder AddSyntaxTrees(IEnumerable<SyntaxTree> trees)
		{
			CollectionHelpers.Add(ref _trees, trees);
			return this;
		}

		public Builder WithSyntaxTrees(SyntaxTree tree)
		{
			CollectionHelpers.With(ref _trees, tree);
			return this;
		}

		public Builder WithSyntaxTrees(IEnumerable<SyntaxTree> trees)
		{
			CollectionHelpers.With(ref _trees, trees);
			return this;
		}

		public Builder AddReferences(MetadataReference reference)
		{
			CollectionHelpers.Add(ref _references, reference);
			return this;
		}

		public Builder AddReferences(IEnumerable<MetadataReference> references)
		{
			CollectionHelpers.Add(ref _references, references);
			return this;
		}

		public Builder WithReferences(MetadataReference reference)
		{
			CollectionHelpers.With(ref _references, reference);
			return this;
		}

		public Builder WithReferences(IEnumerable<MetadataReference> references)
		{
			CollectionHelpers.With(ref _references, references);
			return this;
		}

		public Builder WithAssemblyName(string assemblyName)
		{
			ArgumentException.ThrowIfNullOrWhiteSpace(assemblyName);
			_assemblyName = assemblyName;
			return this;
		}

		public Builder WithOptions(CompilationOptions options)
		{
			ArgumentNullException.ThrowIfNull(options);
			_options = options;
			return this;
		}

		public Compilation Build()
		{
			return new(_assemblyName, _options ?? CompilationOptions.Default, _trees ?? [], _references ?? []); 
		}

		public static implicit operator Compilation(Builder self)
		{
			return self.Build();
		}
	}
}
