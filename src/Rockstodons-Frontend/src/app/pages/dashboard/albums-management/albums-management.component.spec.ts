import { ComponentFixture, TestBed } from '@angular/core/testing';

import { AlbumsManagementComponent } from './albums-management.component';

describe('AlbumsManagementComponent', () => {
  let component: AlbumsManagementComponent;
  let fixture: ComponentFixture<AlbumsManagementComponent>;

  beforeEach(async () => {
    await TestBed.configureTestingModule({
      declarations: [ AlbumsManagementComponent ]
    })
    .compileComponents();

    fixture = TestBed.createComponent(AlbumsManagementComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
