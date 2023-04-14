import { ComponentFixture, TestBed } from '@angular/core/testing';

import { PerformersManagementComponent } from './performers-management.component';

describe('PerformersManagementComponent', () => {
  let component: PerformersManagementComponent;
  let fixture: ComponentFixture<PerformersManagementComponent>;

  beforeEach(async () => {
    await TestBed.configureTestingModule({
      declarations: [ PerformersManagementComponent ]
    })
    .compileComponents();

    fixture = TestBed.createComponent(PerformersManagementComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
