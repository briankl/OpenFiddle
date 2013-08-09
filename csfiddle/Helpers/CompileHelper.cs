﻿using System.CodeDom.Compiler;
using System.IO;
using System.Linq;
using System.Reflection;
using Microsoft.CSharp;

namespace csfiddle.Helpers
{
    public class CompileHelper
    {
        public static string CompileAndRun(string code)
        {
            var compilerParams = new CompilerParameters
            {
                GenerateInMemory = false,
                TreatWarningsAsErrors = false,
                GenerateExecutable = false,
                CompilerOptions = "/optimize"
            };

            string[] references = { "System.dll" };
            compilerParams.ReferencedAssemblies.AddRange(references);

            var provider = new CSharpCodeProvider();
            var compile = provider.CompileAssemblyFromSource(compilerParams, code);

            if (compile.Errors.HasErrors)
            {
                return compile.Errors.Cast<CompilerError>().Aggregate("Compile error: ", (current, ce) => current
                                                                                                          + string.Format("Line: {0}<br />Column: {1}<br />Error Code: {2}<br />Error Text: {3}<br />",
                                                                                                              ce.Line, ce.Column, ce.ErrorNumber, ce.ErrorText));
            }

            var sandbox = Sandbox.Create();
            return sandbox.Execute(compile.PathToAssembly);
        }
    }
}