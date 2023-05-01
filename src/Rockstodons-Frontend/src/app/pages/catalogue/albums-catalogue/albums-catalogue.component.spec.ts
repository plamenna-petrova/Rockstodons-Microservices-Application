import { ComponentFixture, TestBed } from '@angular/core/testing';

import { AlbumsCatalogueComponent } from './albums-catalogue.component';

describe('AlbumsCatalogueComponent', () => {
  let component: AlbumsCatalogueComponent;
  let fixture: ComponentFixture<AlbumsCatalogueComponent>;

  beforeEach(async () => {
    await TestBed.configureTestingModule({
      declarations: [ AlbumsCatalogueComponent ]
    })
    .compileComponents();

    fixture = TestBed.createComponent(AlbumsCatalogueComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
