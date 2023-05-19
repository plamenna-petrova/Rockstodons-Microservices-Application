import { ComponentFixture, TestBed } from '@angular/core/testing';

import { PerformersDetailsComponent } from './performers-details.component';

describe('PerformersDetailsComponent', () => {
  let component: PerformersDetailsComponent;
  let fixture: ComponentFixture<PerformersDetailsComponent>;

  beforeEach(async () => {
    await TestBed.configureTestingModule({
      declarations: [ PerformersDetailsComponent ]
    })
    .compileComponents();

    fixture = TestBed.createComponent(PerformersDetailsComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
