import { TestBed } from '@angular/core/testing';
import { CanActivateFn } from '@angular/router';

import { prestamoGuard } from './logged.guard';

describe('prestamoGuard', () => {
  const executeGuard: CanActivateFn = (...guardParameters) => 
      TestBed.runInInjectionContext(() => prestamoGuard(...guardParameters));

  beforeEach(() => {
    TestBed.configureTestingModule({});
  });

  it('should be created', () => {
    expect(executeGuard).toBeTruthy();
  });
});
