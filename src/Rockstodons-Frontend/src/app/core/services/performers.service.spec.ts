import { TestBed } from '@angular/core/testing';

import { PerformersService } from './performers.service';

describe('PerformersService', () => {
  let service: PerformersService;

  beforeEach(() => {
    TestBed.configureTestingModule({});
    service = TestBed.inject(PerformersService);
  });

  it('should be created', () => {
    expect(service).toBeTruthy();
  });
});
