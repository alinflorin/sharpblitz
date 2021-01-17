import { Injectable } from '@angular/core';
import { DotnetService } from './dotnet.service';
import { RunnerInput } from '../models/runner-input';
import { Observable } from 'rxjs';

@Injectable({
  providedIn: 'root'
})
export class RunnerService {

  constructor(private dotnet: DotnetService) { }

  loadAndRun(input: RunnerInput): Observable<any> {
    return this.dotnet.invoke<any>('SharpBlitz', 'LoadAndRun', input);
  }
}
