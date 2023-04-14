import { ComponentFixture, TestBed } from '@angular/core/testing';

import { AlbumTypesManagementComponent } from './album-types-management.component';

describe('AlbumTypesManagementComponent', () => {
  let component: AlbumTypesManagementComponent;
  let fixture: ComponentFixture<AlbumTypesManagementComponent>;

  beforeEach(async () => {
    await TestBed.configureTestingModule({
      declarations: [ AlbumTypesManagementComponent ]
    })
    .compileComponents();

    fixture = TestBed.createComponent(AlbumTypesManagementComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
