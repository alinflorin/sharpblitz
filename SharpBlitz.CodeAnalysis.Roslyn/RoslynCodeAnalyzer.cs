using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.Completion;
using Microsoft.CodeAnalysis.Host.Mef;
using Microsoft.CodeAnalysis.Text;
using SharpBlitz.CodeAnalysis.Abstractions;
using SharpBlitz.Common;
using SharpBlitz.Common.Assembly;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Threading.Tasks;

namespace SharpBlitz.CodeAnalysis.Roslyn
{
    public class RoslynCodeAnalyzer : ICodeAnalyzer
    {
        private readonly IdeStore _ideStore;
        private AdhocWorkspace _workspace;
        private Project _project;
        private Document _document;

        public RoslynCodeAnalyzer(IdeStore ideStore)
        {
            _ideStore = ideStore;
        }

        public async Task<CodeAnalysisResult> GetIntellisense(CodeAnalysisInput input)
        {
            var refs = new List<PortableExecutableReference>();
            var _ = typeof(Microsoft.CodeAnalysis.CSharp.Formatting.CSharpFormattingOptions);

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

            Assembly.Load(_ideStore.GetFromGac(new AssemblyDefinition { AssemblySource = AssemblySource.GAC, Name = "Microsoft.CodeAnalysis.CSharp.Features.dll" }).Source);

            if (_workspace == null)
            {
                _workspace = new AdhocWorkspace();
                var projectInfo = ProjectInfo.Create(ProjectId.CreateNewId(), VersionStamp.Create(), "MyProject", "MyProject", LanguageNames.CSharp)
                    .WithMetadataReferences(refs);
                _project = _workspace.AddProject(projectInfo);
                _document = _workspace.AddDocument(_project.Id, "MyFile.cs", SourceText.From(input.CurrentFile));
            }

            if (refs.Any(r => !_project.MetadataReferences.Contains(r)))
            {
                _project = _project.WithMetadataReferences(refs);
            }

            _document = _document.WithText(SourceText.From(input.CurrentFile));

            var completionService = CompletionService.GetService(_document);
            var results = await completionService.GetCompletionsAsync(_document, input.CursorPosition);
            return new CodeAnalysisResult
            {
                Suggestions = results.Items.Select(x => new CodeAnalysisCompletionItem {
                    Label = x.DisplayText,
                    Description = x.InlineDescription,
                    InsertText = x.Properties != null && x.Properties.ContainsKey("InsertionText") ? x.Properties["InsertionText"] : x.DisplayText,
                    @Type = x.Properties != null && x.Properties.ContainsKey("SymbolKind") ?
                        Convert((SymbolKind)Enum.Parse(typeof(SymbolKind), x.Properties["SymbolKind"])) : CodeAnalysisCompletionItemType.Text
                }).ToList()
            };
        }

        private CodeAnalysisCompletionItemType Convert(SymbolKind symbolKind)
        {
            switch (symbolKind)
            {
                case SymbolKind.TypeParameter:
                    return CodeAnalysisCompletionItemType.TypeParameter;
                case SymbolKind.Method:
                    return CodeAnalysisCompletionItemType.Method;
                case SymbolKind.Property:
                    return CodeAnalysisCompletionItemType.Property;
                case SymbolKind.Field:
                    return CodeAnalysisCompletionItemType.Field;
                case SymbolKind.Event:
                    return CodeAnalysisCompletionItemType.Event;
                case SymbolKind.Parameter:
                    return CodeAnalysisCompletionItemType.Variable;
                case SymbolKind.Local:
                    return CodeAnalysisCompletionItemType.Variable;
                case SymbolKind.Label:
                    return CodeAnalysisCompletionItemType.Text;
                case SymbolKind.Namespace:
                    return CodeAnalysisCompletionItemType.Module;
                case SymbolKind.NamedType:
                    return CodeAnalysisCompletionItemType.Class;
                case SymbolKind.ErrorType:
                    return CodeAnalysisCompletionItemType.Class;
                case SymbolKind.ArrayType:
                    return CodeAnalysisCompletionItemType.Class;
                case SymbolKind.PointerType:
                    return CodeAnalysisCompletionItemType.Reference;
                case SymbolKind.Assembly:
                    return CodeAnalysisCompletionItemType.Module;
                case SymbolKind.NetModule:
                    return CodeAnalysisCompletionItemType.Module;
                case SymbolKind.Discard:
                    return CodeAnalysisCompletionItemType.Variable;
                case SymbolKind.DynamicType:
                    return CodeAnalysisCompletionItemType.Class;
                case SymbolKind.Preprocessing:
                    return CodeAnalysisCompletionItemType.Constant;
                case SymbolKind.RangeVariable:
                    return CodeAnalysisCompletionItemType.Variable;
            }
            return CodeAnalysisCompletionItemType.Text;
        }
    }
}
