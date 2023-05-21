import { ComponentFixture, TestBed } from '@angular/core/testing';

import { StreamsManagementComponent } from './streams-management.component';

describe('StreamsManagementComponent', () => {
  let component: StreamsManagementComponent;
  let fixture: ComponentFixture<StreamsManagementComponent>;

  beforeEach(async () => {
    await TestBed.configureTestingModule({
      declarations: [ StreamsManagementComponent ]
    })
    .compileComponents();

    fixture = TestBed.createComponent(StreamsManagementComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
