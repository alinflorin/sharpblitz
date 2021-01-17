using SharpBlitz.Common.Assembly;
using System;
using System.Collections.Generic;

namespace SharpBlitz.Compiler.Abstractions
{
    public class CompilationInput
    {
        public string AssemblyName { get; set; } = Guid.NewGuid().ToString() + ".dll";
        public IEnumerable<string> Sources { get; set; }
        public bool DebugMode { get; set; } = true;
        public ProgramType ProgramType { get; set; } = ProgramType.ConsoleApp;
        public IEnumerable<AssemblyDefinition> AdditionalReferences { get; set; }
    }

    public enum ProgramType
    {
        ConsoleApp = 0,
        ClassLibrary = 1
    }
}