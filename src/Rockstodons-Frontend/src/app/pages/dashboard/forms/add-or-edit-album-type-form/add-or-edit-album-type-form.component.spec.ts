import { ComponentFixture, TestBed } from '@angular/core/testing';

import { AddOrEditAlbumTypeFormComponent } from './add-or-edit-album-type-form.component';

describe('AddOrEditAlbumTypeFormComponent', () => {
  let component: AddOrEditAlbumTypeFormComponent;
  let fixture: ComponentFixture<AddOrEditAlbumTypeFormComponent>;

  beforeEach(async () => {
    await TestBed.configureTestingModule({
      declarations: [ AddOrEditAlbumTypeFormComponent ]
    })
    .compileComponents();

    fixture = TestBed.createComponent(AddOrEditAlbumTypeFormComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
