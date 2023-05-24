import { ComponentFixture, TestBed } from '@angular/core/testing';

import { StreamsCatalogueComponent } from './streams-catalogue.component';

describe('StreamsCatalogueComponent', () => {
  let component: StreamsCatalogueComponent;
  let fixture: ComponentFixture<StreamsCatalogueComponent>;

  beforeEach(async () => {
    await TestBed.configureTestingModule({
      declarations: [ StreamsCatalogueComponent ]
    })
    .compileComponents();

    fixture = TestBed.createComponent(StreamsCatalogueComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
