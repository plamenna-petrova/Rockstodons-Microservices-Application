import { ComponentFixture, TestBed } from '@angular/core/testing';

import { AddOrEditGenreFormComponent } from './add-or-edit-genre-form.component';

describe('AddOrEditGenreFormComponent', () => {
  let component: AddOrEditGenreFormComponent;
  let fixture: ComponentFixture<AddOrEditGenreFormComponent>;

  beforeEach(async () => {
    await TestBed.configureTestingModule({
      declarations: [ AddOrEditGenreFormComponent ]
    })
    .compileComponents();

    fixture = TestBed.createComponent(AddOrEditGenreFormComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
