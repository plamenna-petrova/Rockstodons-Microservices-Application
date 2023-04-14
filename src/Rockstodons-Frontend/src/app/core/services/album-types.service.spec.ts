import { TestBed } from '@angular/core/testing';

import { AlbumTypesService } from './album-types.service';

describe('AlbumTypesService', () => {
  let service: AlbumTypesService;

  beforeEach(() => {
    TestBed.configureTestingModule({});
    service = TestBed.inject(AlbumTypesService);
  });

  it('should be created', () => {
    expect(service).toBeTruthy();
  });
});
