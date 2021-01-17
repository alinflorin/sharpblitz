import { Component, OnInit, OnDestroy, NgZone, ViewChild, ElementRef } from '@angular/core';
import { CompilerService } from '../services/compiler.service';
import { RunnerService } from '../services/runner.service';
import { ProgramType } from '../models/compilation-input';
import { CompilationResult } from '../models/compilation-result';
import { LogMessage } from '../models/log-message';
import { AssemblyDefinition, AssemblySource } from '../models/assembly-definition';
import { DependenciesService } from '../services/dependencies.service';
import { CodeAnalysisService } from '../services/code-analysis.service';
import { EditorComponent as MonacoEditorComponent } from 'ngx-monaco-editor';
import { map } from 'rxjs/operators';

@Component({
  selector: 'app-editor',
  templateUrl: './editor.component.html',
  styleUrls: ['./editor.component.scss']
})
export class EditorComponent implements OnInit, OnDestroy {

  gacDependencies: string[] = [];
  lastSuccessfulCompilation: CompilationResult;
  private oldConsoleLog = window.console.log;
  private oldConsoleError = window.console.error;
  @ViewChild('console', { static: true, read: ElementRef }) console: ElementRef<HTMLDivElement>;
  @ViewChild('editor', { static: true }) editor: MonacoEditorComponent;
  logStream: LogMessage[] = [];

  code = `using System;
using System.Threading.Tasks;

namespace MyNamespace {
  public class MyClass {
    public static async Task Main(string[] args) {
      Console.WriteLine("test");
      await Task.CompletedTask;
    }
  }
}`;

  editorOptions = {
    theme: 'vs-dark',
    language: 'csharp',
    automaticLayout: true,
    quickSuggestions: false
  };

  constructor(private c: CompilerService,
    private d: DependenciesService,
    private a: CodeAnalysisService,
    private r: RunnerService, private zone: NgZone) { }

  ngOnInit() {
    window.console.log = (message?: any, ...params: any[]) => {
      this.logConsole(message, params);
      this.oldConsoleLog(message, params);
    };

    window.console.error = (message?: any, ...params: any[]) => {
      this.logConsole(message, params, true);
      this.oldConsoleError(message, params);
    };

  }

  onEditorInit(editor: any) {

    monaco.languages.registerCompletionItemProvider('csharp', {
      provideCompletionItems: (model, position) => {
        let additionalRefs: AssemblyDefinition[] = [];
        additionalRefs = additionalRefs.concat(this.d.preloadedGacDependencies.map(gd => {
          return {
            name: gd,
            assemblySource: AssemblySource.GAC
          } as AssemblyDefinition;
        }));
        additionalRefs = additionalRefs.concat(this.d.uploadedDependencies.map(gd => {
          return {
            name: gd,
            assemblySource: AssemblySource.InPlace
          } as AssemblyDefinition;
        }));
        const line = position.lineNumber;
        const col = position.column;
        const textUntilPosition = model.getValueInRange({ startLineNumber: 1, startColumn: 1, endLineNumber: line, endColumn: col });
        const currentPos = textUntilPosition.length;

        return new Promise<monaco.languages.CompletionList>((accept, reject) => {
          this.a.getIntellisense({
            additionalReferences: additionalRefs,
            currentFile: model.getValue(),
            cursorPosition: currentPos
          }).pipe(map(x => {
            return {
              incomplete: false,
              suggestions: x.suggestions.map(s => {
                return {
                  label: s.label,
                  insertText: s.insertText,
                  range: new monaco.Range(line, col, line + s.insertText.length, col),
                  kind: s.type as any,
                  detail: s.description
                } as monaco.languages.CompletionItem;
              }),
            } as monaco.languages.CompletionList;
          })).subscribe(x => {
            accept(x);
          }, e => { reject(e); });
        });

      }
    });
  }

  private logConsole(message: string, params: any[], error: boolean = false) {
    this.zone.run(() => {
      this.logStream.push({
        message: `${message} ${params == null ? '' : params.map(x => x.toString())}`,
        isError: error
      });
      setTimeout(() => {
        this.console.nativeElement.scrollTop = this.console.nativeElement.scrollHeight;
      });
    });
  }

  ngOnDestroy() {
    window.console.log = this.oldConsoleLog;
    window.console.error = this.oldConsoleError;
  }

  compile() {
    this.lastSuccessfulCompilation = null;
    let additionalRefs: AssemblyDefinition[] = [];
    additionalRefs = additionalRefs.concat(this.d.preloadedGacDependencies.map(gd => {
      return {
        name: gd,
        assemblySource: AssemblySource.GAC
      } as AssemblyDefinition;
    }));
    additionalRefs = additionalRefs.concat(this.d.uploadedDependencies.map(gd => {
      return {
        name: gd,
        assemblySource: AssemblySource.InPlace
      } as AssemblyDefinition;
    }));

    this.c.compile({
      assemblyName: 'Test.dll',
      debugMode: true,
      additionalReferences: additionalRefs,
      programType: ProgramType.ConsoleApp,
      sources: [this.code]
    }).subscribe(cResult => {
      if (!cResult.success) {
        console.error(cResult.errors);
        return;
      }
      console.log('Compilation successful!');
      this.lastSuccessfulCompilation = cResult;
    },
      e => {
        console.error('Compilation failed! Result: ', e);
      });
  }

  run() {
    if (this.lastSuccessfulCompilation == null) {
      console.error('Please compile your code!');
      return;
    }
    let additionalRefs: AssemblyDefinition[] = [];
    additionalRefs = additionalRefs.concat(this.d.preloadedGacDependencies.map(gd => {
      return {
        name: gd,
        assemblySource: AssemblySource.GAC
      } as AssemblyDefinition;
    }));
    additionalRefs = additionalRefs.concat(this.d.uploadedDependencies.map(gd => {
      return {
        name: gd,
        assemblySource: AssemblySource.InPlace
      } as AssemblyDefinition;
    }));
    this.r.loadAndRun({
      mainAssembly: {
        name: 'Test.dll',
        source: this.lastSuccessfulCompilation.assembly.source
      },
      additionalAssemblies: additionalRefs
    }).subscribe(result => {
      console.log('Ran.');
      console.log(result);
    }, e => {
      console.error(e);
    });
  }
}
