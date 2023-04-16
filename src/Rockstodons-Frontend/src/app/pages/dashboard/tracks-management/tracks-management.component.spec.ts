import { ComponentFixture, TestBed } from '@angular/core/testing';

import { TracksManagementComponent } from './tracks-management.component';

describe('TracksManagementComponent', () => {
  let component: TracksManagementComponent;
  let fixture: ComponentFixture<TracksManagementComponent>;

  beforeEach(async () => {
    await TestBed.configureTestingModule({
      declarations: [ TracksManagementComponent ]
    })
    .compileComponents();

    fixture = TestBed.createComponent(TracksManagementComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
