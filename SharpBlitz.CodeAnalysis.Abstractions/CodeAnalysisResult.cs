using System;
using System.Collections.Generic;
using System.Text;

namespace SharpBlitz.CodeAnalysis.Abstractions
{
    public class CodeAnalysisResult
    {
        public IEnumerable<CodeAnalysisCompletionItem> Suggestions { get; set; }
    }
}
