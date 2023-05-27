import { APP_INITIALIZER, NgModule } from '@angular/core';
import { BrowserModule } from '@angular/platform-browser';

import { AppRoutingModule } from './app-routing.module';
import { AppComponent } from './app.component';
import { HTTP_INTERCEPTORS } from '@angular/common/http';
import { FormsModule, ReactiveFormsModule } from '@angular/forms';
import { NzFormModule } from 'ng-zorro-antd/form';
import { NzInputModule } from 'ng-zorro-antd/input';
import { NzButtonModule } from 'ng-zorro-antd/button';
import { NzIconModule } from 'ng-zorro-antd/icon';

import { UserOutline, LockOutline } from '@ant-design/icons-angular/icons';
import { IconDefinition } from '@ant-design/icons-angular';
import { IconsProviderModule } from './icons-provider.module';
import { NzLayoutModule } from 'ng-zorro-antd/layout';
import { NzMenuModule } from 'ng-zorro-antd/menu';
import { NzBreadCrumbModule } from 'ng-zorro-antd/breadcrumb';
import { NzNotificationModule } from 'ng-zorro-antd/notification';
import { NzPageHeaderModule } from 'ng-zorro-antd/page-header';
import { NzCardModule } from 'ng-zorro-antd/card';
import { NzSpaceModule } from 'ng-zorro-antd/space';
import { NzDividerModule } from 'ng-zorro-antd/divider';
import { NzCarouselModule } from 'ng-zorro-antd/carousel';
import { NzImageModule } from 'ng-zorro-antd/image';
import { NzPaginationModule } from 'ng-zorro-antd/pagination';
import { NzRadioModule } from 'ng-zorro-antd/radio';
import { NzDescriptionsModule } from 'ng-zorro-antd/descriptions';
import { NzListModule } from 'ng-zorro-antd/list';
import { NzCommentModule } from 'ng-zorro-antd/comment';
import { HomeComponent } from './content/home/home.component';
import { SharedModule } from './common/shared/shared.module';

import { BrowserAnimationsModule } from '@angular/platform-browser/animations';
import { FileSaverModule } from 'ngx-filesaver';
import { AlbumsCatalogueComponent } from './pages/catalogue/albums-catalogue/albums-catalogue.component';
import { PerformersCatalogueComponent } from './pages/catalogue/performers-catalogue/performers-catalogue.component';
import { GenresCatalogueComponent } from './pages/catalogue/genres-catalogue/genres-catalogue.component';
import { NavbarComponent } from './content/navbar/navbar.component';
import { ShouldLoginComponent } from './should-login/should-login.component';
import { DefaultOAuthInterceptor } from 'angular-oauth2-oidc';
import { CoreModule } from './core.module';
import { ErrorHandlerInterceptor } from './core/interceptors/error-handler.interceptor';
import { RouteReuseStrategy } from '@angular/router';
import { RouteReusableStrategy } from './core/utils/route-reusable-strategy';
import { AlbumDetailsComponent } from './pages/catalogue/details/album-details/album-details.component';
import { PerformersDetailsComponent } from './pages/catalogue/details/performers-details/performers-details.component';
import { StreamsCatalogueComponent } from './pages/catalogue/streams-catalogue/streams-catalogue.component';
import { StreamDetailsComponent } from './pages/catalogue/details/stream-details/stream-details.component';
import { FooterComponent } from './content/footer/footer.component';

import { NZ_I18N, en_US } from 'ng-zorro-antd/i18n';

import { registerLocaleData } from '@angular/common';
import en from '@angular/common/locales/en';
registerLocaleData(en);

const customLanguagePack = {
  en_US,
  ...{
    Pagination: {
      items_per_page: 'results per page'
    }
  }
};

const icons: IconDefinition[] = [
  UserOutline,
  LockOutline
];

@NgModule({
  declarations: [
    AppComponent,
    HomeComponent,
    NavbarComponent,
    AlbumsCatalogueComponent,
    PerformersCatalogueComponent,
    GenresCatalogueComponent,
    ShouldLoginComponent,
    AlbumDetailsComponent,
    PerformersDetailsComponent,
    StreamsCatalogueComponent,
    StreamDetailsComponent,
    FooterComponent,
  ],
  imports: [
    BrowserModule,
    AppRoutingModule,
    FormsModule,
    ReactiveFormsModule,
    NzFormModule,
    NzInputModule,
    NzButtonModule,
    NzNotificationModule,
    NzPageHeaderModule,
    NzCardModule,
    NzSpaceModule,
    NzDividerModule,
    NzCarouselModule,
    NzImageModule,
    NzPaginationModule,
    NzRadioModule,
    NzDescriptionsModule,
    NzListModule,
    NzCommentModule,
    NzIconModule.forChild(icons),
    BrowserAnimationsModule,
    IconsProviderModule,
    NzLayoutModule,
    NzMenuModule,
    NzBreadCrumbModule,
    FileSaverModule,
    SharedModule,
    CoreModule.forRoot()
  ],
  providers: [
    {
      provide: NZ_I18N,
      useValue: en_US
    },
    {
      provide: HTTP_INTERCEPTORS,
      useClass: DefaultOAuthInterceptor,
      multi: true
    },
    {
      provide: HTTP_INTERCEPTORS,
      useClass: ErrorHandlerInterceptor,
      multi: true,
    },
    {
      provide: RouteReuseStrategy,
      useClass: RouteReusableStrategy,
    },
    {
      provide: NZ_I18N,
      useValue: customLanguagePack
    }
  ],
  bootstrap: [AppComponent],
})
export class AppModule { }
