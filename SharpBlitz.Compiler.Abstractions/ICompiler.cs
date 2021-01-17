using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace SharpBlitz.Compiler.Abstractions
{
    public interface ICompiler
    {
        Task<CompilationResult> Compile(CompilationInput input);
    }
}
