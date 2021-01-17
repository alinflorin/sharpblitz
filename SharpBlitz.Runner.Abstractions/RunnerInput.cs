using SharpBlitz.Common.Assembly;
using System.Collections.Generic;
namespace SharpBlitz.Runner.Abstractions
{
    public class RunnerInput
    {
        public AssemblyDefinition MainAssembly { get; set; }
        public string MethodName { get; set; }
        public IEnumerable<AssemblyDefinition> AdditionalAssemblies { get; set; }
    }
}