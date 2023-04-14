import { ComponentFixture, TestBed } from '@angular/core/testing';

import { GenresManagementComponent } from './genres-management.component';

describe('GenresManagementComponent', () => {
  let component: GenresManagementComponent;
  let fixture: ComponentFixture<GenresManagementComponent>;

  beforeEach(async () => {
    await TestBed.configureTestingModule({
      declarations: [ GenresManagementComponent ]
    })
    .compileComponents();

    fixture = TestBed.createComponent(GenresManagementComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
