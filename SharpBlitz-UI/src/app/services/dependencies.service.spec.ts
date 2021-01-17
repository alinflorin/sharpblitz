import { TestBed } from '@angular/core/testing';

import { DependenciesService } from './dependencies.service';

describe('DependenciesService', () => {
  beforeEach(() => TestBed.configureTestingModule({}));

  it('should be created', () => {
    const service: DependenciesService = TestBed.get(DependenciesService);
    expect(service).toBeTruthy();
  });
});
