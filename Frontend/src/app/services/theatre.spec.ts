import { TestBed } from '@angular/core/testing';
import { Theatre } from './theatre';

describe('Theatre', () => {
  let service: Theatre;

  beforeEach(() => {
    TestBed.configureTestingModule({});
    service = TestBed.inject(Theatre);
  });

  it('should be created', () => {
    expect(service).toBeTruthy();
  });
});
