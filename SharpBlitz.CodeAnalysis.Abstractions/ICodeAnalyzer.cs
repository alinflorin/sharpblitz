using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace SharpBlitz.CodeAnalysis.Abstractions
{
    public interface ICodeAnalyzer
    {
        Task<CodeAnalysisResult> GetIntellisense(CodeAnalysisInput input);
    }
}
