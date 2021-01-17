import { AssemblyDefinition } from './assembly-definition';

export interface CompilationInput {
  assemblyName: string;
  sources: string[];
  debugMode: boolean;
  programType: ProgramType;
  additionalReferences?: AssemblyDefinition[];
}

export enum ProgramType {
  ConsoleApp = 0,
  ClassLibrary = 1
}
