import { AssemblyDefinition } from './assembly-definition';

export interface CodeAnalysisInput {
  currentFile: string;
  additionalReferences?: AssemblyDefinition[];
  cursorPosition: number;
}
