import { APP_INITIALIZER, ModuleWithProviders, NgModule, Optional, SkipSelf } from '@angular/core';
import { CommonModule } from '@angular/common';
import { HttpClientModule } from '@angular/common/http';
import { RouteReuseStrategy, RouterModule } from '@angular/router';

import { RouteReusableStrategy } from '../app/core/utils/route-reusable-strategy';

// OIDC Integration
import { AuthConfig, OAuthModule, OAuthModuleConfig, OAuthStorage } from 'angular-oauth2-oidc';
import { authAppInitializerFactory } from '../app/core/utils/auth-app-initializer.factory';
import { authConfig } from '../app/core/utils/auth-config';
import { AuthGuardWithForcedLogin } from '../app/core/utils/auth-guard-with-forced-login.service';
import { AuthGuard } from '../app/core/utils/auth-guard.service';
import { authModuleConfig } from '../app/core/utils/auth-module-config';
import { OAuth2Service } from '../app/core/services/oauth2.service';
import { RoleGuard } from '../app/core/utils/role-guard.service';

// We need a factory since localStorage is not available at AOT build time
export function storageFactory(): OAuthStorage {
  return localStorage;
}

@NgModule({
  imports: [
    CommonModule,
    HttpClientModule,
    OAuthModule.forRoot({
      resourceServer: {
        allowedUrls: ['https://localhost:7246/api/v1'],
        sendAccessToken: true
      }
    }),
    RouterModule
  ],
  providers: [
    OAuth2Service,
    AuthGuard,
    RoleGuard,
    AuthGuardWithForcedLogin,
    {
      provide: RouteReuseStrategy,
      useClass: RouteReusableStrategy,
    },
  ],
})
export class CoreModule {
  static forRoot(): ModuleWithProviders<CoreModule> {
    return {
      ngModule: CoreModule,
      providers: [
        {
          provide: APP_INITIALIZER,
          useFactory: authAppInitializerFactory,
          deps: [OAuth2Service],
          multi: true
        },
        {
          provide: AuthConfig,
          useValue: authConfig
        },
        {
          provide: OAuthModuleConfig,
          useValue: authModuleConfig
        },
        {
          provide: OAuthStorage,
          useFactory: storageFactory
        },
      ],
    };
  }
  constructor(@Optional() @SkipSelf() parentModule: CoreModule) {
    // Import guard
    if (parentModule) {
      throw new Error(`${parentModule} has already been loaded. Import Core module in the AppModule only.`);
    }
  }
}
