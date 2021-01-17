using System;
using System.Collections.Generic;
using System.Text;

namespace SharpBlitz.Common.Assembly
{
    public class UnloadGacAssemblyResult
    {
        public string Name { get; set; }
        public bool Success { get; set; } = true;
    }
}
