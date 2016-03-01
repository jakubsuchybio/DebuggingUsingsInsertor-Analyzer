using System;
using System.Collections.Generic;
using System.Collections.Immutable;
using System.Linq;
using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp.Syntax;
using Microsoft.CodeAnalysis.Diagnostics;

namespace USI
{
	[DiagnosticAnalyzer(LanguageNames.CSharp)]
	public class USI0001RequredUsingsAreNotPresent : DiagnosticAnalyzer
	{
		public const string DiagnosticId = "USI0001";

		private const string Category = "Usings";
		private const string Description = "All code files should contain usings: System; System.Linq; System.Collections.Generic";
		private const string Title = "Usings doesn't contain required ones.";
		private static readonly string MessageFormat = "Missing these usings:" + Environment.NewLine + "{0}";
		private static readonly DiagnosticDescriptor Rule = new DiagnosticDescriptor(DiagnosticId , Title , MessageFormat , Category , DiagnosticSeverity.Info , isEnabledByDefault: true , description: Description);

		public override ImmutableArray<DiagnosticDescriptor> SupportedDiagnostics => ImmutableArray.Create(Rule);

		public override void Initialize(AnalysisContext context)
		{
			context.RegisterSyntaxTreeAction(AnalyzeUsings);
		}

		private void AnalyzeUsings(SyntaxTreeAnalysisContext context)
		{
			if( context.IsGenerated() )
				return;

			var root = context.Tree.GetRoot() as CompilationUnitSyntax;
			if( root == null )
				return;

			var reqUsings = new HashSet<string>
			{
				"System",
				"System.Collections.Generic",
				"System.Linq"
			};

			foreach( var item in root.Usings )
			{
				reqUsings.Remove(item.Name.ToString());

				if( reqUsings.Count == 0 )
					return;
			}

			var message = string.Join( Environment.NewLine, reqUsings );
			var location = root.Usings.Count == 0 ? root.GetLocation() : root.Usings[0].GetLocation();

			var diagnostic = Diagnostic.Create( Rule, location, message );
			context.ReportDiagnostic(diagnostic);
		}
	}
}