using SharpBlitz.Common.Assembly;
using System;
using System.Collections.Generic;
using System.Text;

namespace SharpBlitz.Compiler.Abstractions
{
    public class CompilationResult
    {
        public AssemblyDefinition Assembly { get; set; }
        public bool Success { get; set; } = true;
        public IEnumerable<string> Errors { get; set; }
    }
}
