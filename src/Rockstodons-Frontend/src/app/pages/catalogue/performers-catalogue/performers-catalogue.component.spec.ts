import { ComponentFixture, TestBed } from '@angular/core/testing';

import { PerformersCatalogueComponent } from './performers-catalogue.component';

describe('PerformersCatalogueComponent', () => {
  let component: PerformersCatalogueComponent;
  let fixture: ComponentFixture<PerformersCatalogueComponent>;

  beforeEach(async () => {
    await TestBed.configureTestingModule({
      declarations: [ PerformersCatalogueComponent ]
    })
    .compileComponents();

    fixture = TestBed.createComponent(PerformersCatalogueComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
