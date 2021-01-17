import { Injectable } from '@angular/core';
import { Observable, of } from 'rxjs';
import { map, tap } from 'rxjs/operators';
import { DotnetService } from './dotnet.service';
import { AssemblyDefinition } from '../models/assembly-definition';
import { PreloadGacAssemblyResult } from '../models/preload-gac-assembly-result';
import { UnloadGacAssemblyResult } from '../models/unload-gac-assembly-result';
import { PreloadUploadedAssemblyResult } from '../models/preload-uploaded-assembly-result';
import { UnloadUploadedAssemblyResult } from '../models/unload-uploaded-assembly-result';

declare var MONO: any;

@Injectable({
  providedIn: 'root'
})

export class DependenciesService {

  preloadedGacDependencies: string[] = [];
  uploadedDependencies: string[] = [];

  constructor(private dotnet: DotnetService) { }

  get gacDependencies(): Observable<string[]> {
    try {
      return of(MONO).pipe(
        map(x => {
          if (x == null) {
            return [];
          }
          return x.loaded_files as string[];
        }),
        map(x => {
          if (x == null) {
            return [];
          }
          return x.map(z => (z.split('/'))[z.split('/').length - 1]).filter(x => !x.startsWith('SharpBlitz.') && x !== 'mscorlib.dll');
        })
      );
    } catch (err) {
      return of([]);
    }
  }

  preloadGacAssembly(def: AssemblyDefinition) {
    return this.dotnet.invoke<PreloadGacAssemblyResult>('SharpBlitz', 'PreloadGacAssembly', def).pipe(tap(gd => {
      this.preloadedGacDependencies.push(gd.name);
    }));
  }

  unloadGacAssembly(def: AssemblyDefinition) {
    return this.dotnet.invoke<UnloadGacAssemblyResult>('SharpBlitz', 'UnloadGacAssembly', def).pipe(tap(gd => {
      this.preloadedGacDependencies.splice(this.preloadedGacDependencies.indexOf(gd.name), 1);
    }));
  }

  preloadUploadedAssembly(def: AssemblyDefinition) {
    return this.dotnet.invoke<PreloadUploadedAssemblyResult>('SharpBlitz', 'PreloadUploadedAssembly', def).pipe(tap(gd => {
      this.uploadedDependencies.push(gd.name);
    }));
  }

  unloadUploadedAssembly(def: AssemblyDefinition) {
    return this.dotnet.invoke<UnloadUploadedAssemblyResult>('SharpBlitz', 'UnloadUploadedAssembly', def).pipe(tap(gd => {
      this.uploadedDependencies.splice(this.preloadedGacDependencies.indexOf(gd.name), 1);
    }));
  }
}
