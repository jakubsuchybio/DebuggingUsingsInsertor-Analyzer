using System;
using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CodeFixes;
using Microsoft.CodeAnalysis.Diagnostics;
using TestHelper;
using Xunit;

namespace USI.Test
{
	public class Tests : CodeFixVerifier
	{
		const string n = "\r\n";

		const string miss1 = "Missing these usings:" + n + "System";
		const string miss2 = "Missing these usings:" + n + "System.Collections.Generic";
		const string miss3 = "Missing these usings:" + n + "System.Linq";
		const string miss12 = "Missing these usings:" + n + "System" + n + "System.Collections.Generic";
		const string miss13 = "Missing these usings:" + n + "System" + n + "System.Linq";
		const string miss23 = "Missing these usings:" + n + "System.Collections.Generic" + n + "System.Linq";
		const string miss123 = "Missing these usings:" + n + "System" + n + "System.Collections.Generic" + n + "System.Linq";

		const string u1 = "using System;";
		const string u2 = "using System.Collections.Generic;";
		const string u3 = "using System.Linq;";

		const string lt = "//LeadTrail";
		const string tt = "//TrailTrail";

		[Theory]
		[InlineData( "", u1 + n + u2 + n + u3 + n + n )]
		[InlineData( u1, u1 + n + u2 + n + u3 + n + n )]
		[InlineData( u2, u1 + n + u2 + n + u3 + n + n )]
		[InlineData( u3, u1 + n + u2 + n + u3 + n + n )]
		[InlineData( u1 + n + u2, u1 + n + u2 + n + u3 + n + n )]
		[InlineData( u2 + n + u1, u1 + n + u2 + n + u3 + n + n )]
		[InlineData( u1 + n + u3, u1 + n + u2 + n + u3 + n + n )]
		[InlineData( u3 + n + u1, u1 + n + u2 + n + u3 + n + n )]
		[InlineData( u2 + n + u3, u1 + n + u2 + n + u3 + n + n )]
		[InlineData( u3 + n + u2, u1 + n + u2 + n + u3 + n + n )]
		[InlineData( lt + n + u1 + n + u2, lt + n + u1 + n + u2 + n + u3 + n + n )]
		[InlineData( u1 + n + u2 + n + tt, u1 + n + u2 + n + u3 + n + n + tt )]
		public void Should_Give_Fix( string input, string fix )
		{
			VerifyCSharpFix( input, fix, allowNewCompilerDiagnostics: true );
		}

		[Theory]
		[InlineData( u1 + n + u2 + n + u3 )]
		[InlineData( u1 + n + u3 + n + u2 )]
		[InlineData( u2 + n + u1 + n + u3 )]
		[InlineData( u2 + n + u3 + n + u1 )]
		[InlineData( u3 + n + u1 + n + u2 )]
		[InlineData( u3 + n + u2 + n + u1 )]
		public void Should_Not_Return_Diagnostics( string input )
		{
			VerifyCSharpDiagnostic( input );
		}

		[Theory]
		[InlineData( "", miss123, 1, 1 )]
		[InlineData( u1, miss23, 1, 1 )]
		[InlineData( u2, miss13, 1, 1 )]
		[InlineData( u3, miss12, 1, 1 )]
		[InlineData( u1 + n + u2, miss3, 1, 1 )]
		[InlineData( u2 + n + u1, miss3, 1, 1 )]
		[InlineData( u1 + n + u3, miss2, 1, 1 )]
		[InlineData( u3 + n + u1, miss2, 1, 1 )]
		[InlineData( u2 + n + u3, miss1, 1, 1 )]
		[InlineData( u3 + n + u2, miss1, 1, 1 )]
		[InlineData( lt + n + u3 + n + u2, miss1, 2, 1 )]
		[InlineData( u3 + n + u2 + n + tt, miss1, 1, 1 )]
		public void Should_Return_Diagnostics( string input, string msg, int locX, int locY )
		{
			DiagnosticResult expected = CreateDiagnostic( msg, locX, locY );

			VerifyCSharpDiagnostic( input, expected );
		}

		protected override CodeFixProvider GetCSharpCodeFixProvider() => new USI0001CodeFixProvider();

		protected override DiagnosticAnalyzer GetCSharpDiagnosticAnalyzer() => new USI0001RequredUsingsAreNotPresent();

		static DiagnosticResult CreateDiagnostic( string msg, int locX, int locY ) =>
											new DiagnosticResult
											{
												Id = "USI0001",
												Message = msg,
												Severity = DiagnosticSeverity.Info,
												Locations = new[] { new DiagnosticResultLocation( "Test0.cs", locX, locY ) }
											};
	}
}