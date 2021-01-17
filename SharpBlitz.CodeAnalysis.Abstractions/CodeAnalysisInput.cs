using SharpBlitz.Common.Assembly;
using System;
using System.Collections.Generic;
using System.Text;

namespace SharpBlitz.CodeAnalysis.Abstractions
{
    public class CodeAnalysisInput
    {
        public string CurrentFile { get; set; }
        public IEnumerable<AssemblyDefinition> AdditionalReferences { get; set; }
        public int CursorPosition { get; set; }
    }
}
