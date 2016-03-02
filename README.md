# Debugging Usings Insertor Analyzer
This is an example of roslyn analyzer, 
that can create diagnostics when there are missing some usings,
that are required for using debugging data transformation via lambdas and Linqs in Watch or Immediate windows.

Because Microsoft still haven't added support for post-loading libraries like System.Linq when debugging and therefor
it is impossible to use Linq data transformations inside Watch or Immediate windows when debugging.

This extension is available in Visual Studio Gallery of Extensions here:</br>
https://visualstudiogallery.msdn.microsoft.com/de8b7852-d77f-417d-95eb-4b645751de3b

And also as an NuGet package here:</br>
https://www.nuget.org/packages/DebuggingUsingsInsertor/1.0.5904.40201
