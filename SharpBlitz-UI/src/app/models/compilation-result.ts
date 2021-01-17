import { AssemblyDefinition } from './assembly-definition';

export class CompilationResult {
  assembly: AssemblyDefinition;
  errors?: string[];
  success: boolean;
}
