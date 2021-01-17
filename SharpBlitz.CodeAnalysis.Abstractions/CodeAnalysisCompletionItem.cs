using System;
using System.Collections.Generic;
using System.Text;

namespace SharpBlitz.CodeAnalysis.Abstractions
{
    public class CodeAnalysisCompletionItem
    {
        public string Label { get; set; }
        public string InsertText { get; set; }
        public CodeAnalysisCompletionItemType Type { get; set; }
        public string Description { get; set; }
    }
}
