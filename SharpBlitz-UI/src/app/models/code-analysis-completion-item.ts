import { CodeAnalysisCompletionItemType } from './code-analysis-completion-item-type';

export interface CodeAnalysisCompletionItem {
  label: string;
  insertText: string;
  type: CodeAnalysisCompletionItemType;
  description?: string;
}
