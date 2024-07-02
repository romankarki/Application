import { TestBed } from '@angular/core/testing';

import { OfficerServiceService } from './officer-service.service';

describe('OfficerServiceService', () => {
  let service: OfficerServiceService;

  beforeEach(() => {
    TestBed.configureTestingModule({});
    service = TestBed.inject(OfficerServiceService);
  });

  it('should be created', () => {
    expect(service).toBeTruthy();
  });
});
