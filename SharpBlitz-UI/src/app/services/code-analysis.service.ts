import { Injectable } from '@angular/core';
import { CodeAnalysisInput } from '../models/code-analysis-input';
import { Observable } from 'rxjs';
import { CodeAnalysisResult } from '../models/code-analysis-result';
import { DotnetService } from './dotnet.service';

@Injectable({
  providedIn: 'root'
})
export class CodeAnalysisService {

  constructor(private dotnet: DotnetService) { }

  getIntellisense(input: CodeAnalysisInput): Observable<CodeAnalysisResult> {
    return this.dotnet.invoke<CodeAnalysisResult>('SharpBlitz', 'Analyze', input);
  }
}
