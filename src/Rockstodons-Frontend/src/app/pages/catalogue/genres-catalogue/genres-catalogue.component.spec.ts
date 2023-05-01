import { ComponentFixture, TestBed } from '@angular/core/testing';

import { GenresCatalogueComponent } from './genres-catalogue.component';

describe('GenresCatalogueComponent', () => {
  let component: GenresCatalogueComponent;
  let fixture: ComponentFixture<GenresCatalogueComponent>;

  beforeEach(async () => {
    await TestBed.configureTestingModule({
      declarations: [ GenresCatalogueComponent ]
    })
    .compileComponents();

    fixture = TestBed.createComponent(GenresCatalogueComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
