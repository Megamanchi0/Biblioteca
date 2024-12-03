import { ComponentFixture, TestBed } from '@angular/core/testing';

import { GestionPrestamoComponent } from './gestion-prestamo.component';

describe('GestionPrestamoComponent', () => {
  let component: GestionPrestamoComponent;
  let fixture: ComponentFixture<GestionPrestamoComponent>;

  beforeEach(async () => {
    await TestBed.configureTestingModule({
      declarations: [GestionPrestamoComponent]
    })
    .compileComponents();

    fixture = TestBed.createComponent(GestionPrestamoComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
