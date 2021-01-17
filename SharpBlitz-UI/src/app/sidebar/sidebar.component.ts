import { Component, OnInit } from '@angular/core';
import { DependenciesService } from '../services/dependencies.service';
import { AssemblySource } from '../models/assembly-definition';

@Component({
  selector: 'app-sidebar',
  templateUrl: './sidebar.component.html',
  styleUrls: ['./sidebar.component.scss']
})
export class SidebarComponent implements OnInit {
  gacDependencies: string[] = [];
  constructor(public d: DependenciesService) { }

  ngOnInit() {
    this.d.gacDependencies.subscribe(gac => {
      this.gacDependencies = gac;
    });
  }

  isLoadedInGac(asmName: string) {
    return this.d.preloadedGacDependencies.indexOf(asmName) > -1;
  }

  toggleGacLoadingUnloading(asmName: string) {
    if (!this.isLoadedInGac(asmName)) {
      this.d.preloadGacAssembly({
        name: asmName,
        assemblySource: AssemblySource.GAC
      }).subscribe(() => { });
    } else {
      this.d.unloadGacAssembly({
        name: asmName,
        assemblySource: AssemblySource.GAC
      }).subscribe(() => { });
    }
  }

  onDllUploaded(files: FileList) {
    for (let i = 0; i < files.length; i++) {
      const f = files.item(i);
      const fileReader = new FileReader();
      fileReader.onload = () => {
        const bytes = btoa(fileReader.result.toString());
        this.d.preloadUploadedAssembly({
          assemblySource: AssemblySource.InPlace,
          name: f.name,
          source: bytes
        }).subscribe(() => {});
      };
      fileReader.readAsBinaryString(f);
    }
  }

  unloadUploadedDependency(asmName: string) {
    this.d.unloadUploadedAssembly({
      name: asmName,
      assemblySource: AssemblySource.InPlace
    }).subscribe(() => { });
  }
}
