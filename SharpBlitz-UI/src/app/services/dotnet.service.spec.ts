import { TestBed } from '@angular/core/testing';

import { DotnetService } from './dotnet.service';

describe('DotnetService', () => {
  beforeEach(() => TestBed.configureTestingModule({}));

  it('should be created', () => {
    const service: DotnetService = TestBed.get(DotnetService);
    expect(service).toBeTruthy();
  });
});
