# UsingsInsertor Analyzer
This is an example of roslyn analyzer, 
that can create diagnostics when there are missing some usings,
that are required for using debugging data transformation via lambdas and Linqs in Watch or Immediate windows.

Because Microsoft still haven't added support for post-loading libraries like System.Linq when debugging and therefor
it is impossible to use Linq data transformations inside Watch or Immediate windows when debugging.
