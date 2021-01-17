import { TestBed } from '@angular/core/testing';

import { CodeAnalysisService } from './code-analysis.service';

describe('CodeAnalysisService', () => {
  beforeEach(() => TestBed.configureTestingModule({}));

  it('should be created', () => {
    const service: CodeAnalysisService = TestBed.get(CodeAnalysisService);
    expect(service).toBeTruthy();
  });
});
