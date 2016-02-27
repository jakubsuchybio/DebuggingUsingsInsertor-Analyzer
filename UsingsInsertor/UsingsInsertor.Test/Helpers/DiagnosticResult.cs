using System;
using Microsoft.CodeAnalysis;

namespace TestHelper
{
	/// <summary>Struct that stores information about a Diagnostic appearing in a source</summary>
	public struct DiagnosticResult
	{
		DiagnosticResultLocation[] locations;

		public int Column => this.Locations.Length > 0 ? this.Locations[0].Column : -1;

		public string Id { get; set; }

		public int Line => this.Locations.Length > 0 ? this.Locations[0].Line : -1;

		public DiagnosticResultLocation[] Locations
		{
			get
			{
				if( this.locations == null )
				{
					this.locations = new DiagnosticResultLocation[] { };
				}
				return this.locations;
			}

			set
			{
				this.locations = value;
			}
		}

		public string Message { get; set; }
		public string Path => this.Locations.Length > 0 ? this.Locations[0].Path : "";
		public DiagnosticSeverity Severity { get; set; }
	}

	/// <summary>
	/// Location where the diagnostic appears, as determined by path, line number, and column number.
	/// </summary>
	public struct DiagnosticResultLocation
	{
		public DiagnosticResultLocation(string path , int line , int column)
		{
			if( line < -1 )
			{
				throw new ArgumentOutOfRangeException(nameof(line) , "line must be >= -1");
			}

			if( column < -1 )
			{
				throw new ArgumentOutOfRangeException(nameof(column) , "column must be >= -1");
			}

			this.Path = path;
			this.Line = line;
			this.Column = column;
		}

		public int Column { get; }
		public int Line { get; }
		public string Path { get; }
	}
}