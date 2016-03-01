using System;
using System.Collections.Generic;
using System.Collections.Immutable;
using System.Composition;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CodeActions;
using Microsoft.CodeAnalysis.CodeFixes;
using Microsoft.CodeAnalysis.CSharp;
using Microsoft.CodeAnalysis.CSharp.Syntax;

namespace USI
{
	[ExportCodeFixProvider(LanguageNames.CSharp , Name = nameof(USI0001CodeFixProvider)), Shared]
	public class USI0001CodeFixProvider : CodeFixProvider
	{
		private const string title = "Insert usings";

		public sealed override ImmutableArray<string> FixableDiagnosticIds => ImmutableArray.Create(USI0001RequredUsingsAreNotPresent.DiagnosticId);

		public sealed override FixAllProvider GetFixAllProvider() => WellKnownFixAllProviders.BatchFixer;

		public sealed override Task RegisterCodeFixesAsync(CodeFixContext context)
		{
			foreach( var diagnostic in context.Diagnostics )
			{
				context.RegisterCodeFix(
				CodeAction.Create(
					title: title ,
					createChangedDocument: c => GetTransformedUsings(context.Document , diagnostic , c) ,
					equivalenceKey: "USI0001CodeFixProvider") ,
				diagnostic);
			}
			return Task.FromResult<object>(null);
		}

		private UsingDirectiveSyntax GetNewUsing(string name) => SyntaxFactory.UsingDirective(SyntaxFactory.ParseName(name));

		private SyntaxList<UsingDirectiveSyntax> GetSyntaxList(IEnumerable<UsingDirectiveSyntax> list) => new SyntaxList<UsingDirectiveSyntax>().AddRange(list);

		private async Task<Document> GetTransformedUsings(Document document , Diagnostic diagnostic , CancellationToken c)
		{
			var oldRoot = await document.GetSyntaxRootAsync( c ).ConfigureAwait( false ) as CompilationUnitSyntax;

			var reqUsings = new HashSet<string>
			{
				"System",
				"System.Collections.Generic",
				"System.Linq"
			};
			var nonRequiredUsings = oldRoot.Usings.Where( item => !reqUsings.Contains( item.Name.ToString() ) );
			var retUsings = reqUsings.Select( name => GetNewUsing( name ) ).Concat( nonRequiredUsings ).ToArray();

			if( oldRoot.Usings.Count == 0 )
				return document.WithSyntaxRoot(oldRoot.AddUsings(retUsings));

			var leadingTrivia = oldRoot.Usings[0].GetLeadingTrivia();
			var newRoot = oldRoot.WithUsings( GetSyntaxList( retUsings ) ).WithLeadingTrivia( leadingTrivia );

			return document.WithSyntaxRoot(newRoot);
		}
	}
}