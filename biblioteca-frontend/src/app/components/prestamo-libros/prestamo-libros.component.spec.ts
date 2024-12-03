import { ComponentFixture, TestBed } from '@angular/core/testing';

import { PrestamoLibrosComponent } from './prestamo-libros.component';

describe('PrestamoLibrosComponent', () => {
  let component: PrestamoLibrosComponent;
  let fixture: ComponentFixture<PrestamoLibrosComponent>;

  beforeEach(async () => {
    await TestBed.configureTestingModule({
      declarations: [PrestamoLibrosComponent]
    })
    .compileComponents();

    fixture = TestBed.createComponent(PrestamoLibrosComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
