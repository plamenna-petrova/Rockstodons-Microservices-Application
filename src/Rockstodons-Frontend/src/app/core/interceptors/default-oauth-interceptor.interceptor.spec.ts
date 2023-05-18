import { TestBed } from '@angular/core/testing';

import { DefaultOauthInterceptorInterceptor } from './default-oauth-interceptor.interceptor';

describe('DefaultOauthInterceptorInterceptor', () => {
  beforeEach(() => TestBed.configureTestingModule({
    providers: [
      DefaultOauthInterceptorInterceptor
      ]
  }));

  it('should be created', () => {
    const interceptor: DefaultOauthInterceptorInterceptor = TestBed.inject(DefaultOauthInterceptorInterceptor);
    expect(interceptor).toBeTruthy();
  });
});
