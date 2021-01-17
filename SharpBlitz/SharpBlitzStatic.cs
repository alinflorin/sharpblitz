using Microsoft.Extensions.DependencyInjection;
using SharpBlitz.CodeAnalysis.Abstractions;
using SharpBlitz.Common;
using SharpBlitz.Compiler.Abstractions;
using SharpBlitz.Runner.Abstractions;
using System;
using System.IO;

namespace SharpBlitz
{
    public static class SharpBlitzStatic
    {
        public static IServiceProvider Services { get; private set; }
        public static ICompiler Compiler { get; private set; }
        public static IdeStore IdeStore { get; private set; }
        public static ICodeAnalyzer Analyzer { get; private set; }
        public static IRunner Runner { get; private set; }
        public static MemoryStream OutStream { get; private set; }

        public static MemoryStream ErrorStream { get; private set; }
        public static void Init(IServiceProvider services)
        {
            Services = services;
            Compiler = services.GetRequiredService<ICompiler>();
            Runner = services.GetRequiredService<IRunner>();
            IdeStore = services.GetRequiredService<IdeStore>();
            Analyzer = services.GetRequiredService<ICodeAnalyzer>();
            OutStream = new MemoryStream();
            ErrorStream = new MemoryStream();
        }

        public static IServiceScope NewDiScope()
        {
            return Services.CreateScope();
        }
    }
}
