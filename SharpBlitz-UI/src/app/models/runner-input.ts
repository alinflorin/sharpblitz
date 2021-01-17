import { AssemblyDefinition } from './assembly-definition';

export interface RunnerInput {
  methodName?: string;
  additionalAssemblies?: AssemblyDefinition[];
  mainAssembly: AssemblyDefinition;
}
