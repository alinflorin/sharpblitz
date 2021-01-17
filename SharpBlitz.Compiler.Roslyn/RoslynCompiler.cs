using SharpBlitz.Compiler.Abstractions;
using Microsoft.CodeAnalysis.CSharp;
using System.Threading.Tasks;
using Microsoft.CodeAnalysis;
using System.Collections.Generic;
using System.IO;
using Microsoft.CodeAnalysis.Text;
using System.Linq;
using SharpBlitz.Common;
using SharpBlitz.Common.Assembly;
namespace SharpBlitz.Compiler.Roslyn
{
    public class RoslynCompiler : ICompiler
    {
        private readonly IdeStore _ideStore;


        public RoslynCompiler(IdeStore ideStore)
        {
            _ideStore = ideStore;
        }

        public async Task<CompilationResult> Compile(CompilationInput input)
        {
            await Task.CompletedTask;
            var opts = new CSharpCompilationOptions(
                outputKind: input.ProgramType == ProgramType.ClassLibrary ? OutputKind.DynamicallyLinkedLibrary : OutputKind.ConsoleApplication,
                optimizationLevel: input.DebugMode ? OptimizationLevel.Debug : OptimizationLevel.Release
            );

            var refs = new List<PortableExecutableReference>();
            
            foreach (var stored in _ideStore.GetRequiredAsms())
            {
                refs.Add(MetadataReference.CreateFromImage(stored.Source));
            }


            if (input.AdditionalReferences != null)
            {
                foreach (var x in input.AdditionalReferences)
                {
                    if (x.AssemblySource == AssemblySource.GAC)
                    {
                        refs.Add(MetadataReference.CreateFromImage(_ideStore.GetFromGac(x).Source));
                        continue;
                    }
                    if (x.AssemblySource == AssemblySource.InPlace)
                    {
                        refs.Add(MetadataReference.CreateFromImage(_ideStore.GetFromUploaded(x).Source));
                        continue;
                    }
                    if (x.AssemblySource == AssemblySource.NuGet)
                    {
                        // TODO
                        continue;
                    }
                }
            }


            var compilation = CSharpCompilation.Create(
                input.AssemblyName,
                input.Sources.Select(source => SyntaxFactory.ParseSyntaxTree(SourceText.From(source), CSharpParseOptions.Default.WithLanguageVersion(LanguageVersion.Latest))).ToList(),
                refs,
                opts
                );
            using (var fileStream = new MemoryStream())
            {
                using (var pdbStream = new MemoryStream())
                {
                    var emitResult = compilation.Emit(fileStream, pdbStream);
                    return new CompilationResult
                    {
                        Success = emitResult.Success,
                        Assembly = new AssemblyDefinition
                        {
                            Debug = pdbStream.ToArray(),
                            Source = fileStream.ToArray(),
                            Name = input.AssemblyName.Replace(".dll", "")
                        },
                        Errors = emitResult.Diagnostics.Select(d => d.ToString()).ToList()
                    };
                }
            }
        }
    }
}
