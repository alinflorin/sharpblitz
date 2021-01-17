using System;
using System.Linq;
using System.Reflection;
using System.Threading.Tasks;
using SharpBlitz.Common;
using SharpBlitz.Runner.Abstractions;

namespace SharpBlitz.Runner.Roslyn
{
    public class RoslynRunner : IRunner
    {
        private readonly IdeStore _ideStore;

        public RoslynRunner(IdeStore ideStore)
        {
            _ideStore = ideStore;
        }

        public async Task LoadAndRun(RunnerInput input)
        {
            foreach (var dep in _ideStore.GetRequiredAsms())
            {
                Assembly.Load(dep.Source);
            }

            if (input.AdditionalAssemblies != null)
            {
                foreach (var add in input.AdditionalAssemblies)
                {
                    if (add.AssemblySource == Common.Assembly.AssemblySource.GAC)
                    {
                        Assembly.Load(_ideStore.GetFromGac(add).Source);
                        continue;
                    }
                    if (add.AssemblySource == Common.Assembly.AssemblySource.InPlace)
                    {
                        var c = _ideStore.GetFromUploaded(add);
                        Assembly.Load(c.Source);
                        continue;
                    }
                    if (add.AssemblySource == Common.Assembly.AssemblySource.NuGet)
                    {
                        // TODO
                        continue;
                    }
                }
            }

            var asm = Assembly.Load(input.MainAssembly.Source);
            if (asm.EntryPoint.GetParameters().Length > 0)
            {
                asm.EntryPoint.Invoke(null, asm.EntryPoint.GetParameters().Select(p => GetDefault(p.ParameterType)).ToArray());
            } else
            {
                asm.EntryPoint.Invoke(null, null);
            }
            await Task.CompletedTask;
        }

        private static object GetDefault(Type type)
        {
            if (type.IsValueType)
            {
                return Activator.CreateInstance(type);
            }
            return null;
        }
    }
}