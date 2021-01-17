export interface AssemblyDefinition {
  source?: string;
  debug?: string;
  version?: string;
  assemblySource?: AssemblySource;
  name: string;
}

export enum AssemblySource {
  InPlace = 0,
  GAC = 1,
  NuGet = 2
}
