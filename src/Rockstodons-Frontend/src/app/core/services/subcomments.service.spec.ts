import { TestBed } from '@angular/core/testing';

import { SubcommentsService } from './subcomments.service';

describe('SubcommentsService', () => {
  let service: SubcommentsService;

  beforeEach(() => {
    TestBed.configureTestingModule({});
    service = TestBed.inject(SubcommentsService);
  });

  it('should be created', () => {
    expect(service).toBeTruthy();
  });
});
