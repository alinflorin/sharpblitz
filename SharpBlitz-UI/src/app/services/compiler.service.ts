import { Injectable } from '@angular/core';
import { DotnetService } from './dotnet.service';
import { CompilationInput } from '../models/compilation-input';
import { Observable } from 'rxjs';
import { CompilationResult } from '../models/compilation-result';

@Injectable({
  providedIn: 'root'
})
export class CompilerService {

  constructor(private dotnet: DotnetService) { }

  compile(input: CompilationInput): Observable<CompilationResult> {
    return this.dotnet.invoke<CompilationResult>('SharpBlitz', 'Compile', input);
  }
}
