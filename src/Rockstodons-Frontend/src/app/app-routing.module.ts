import { NgModule } from '@angular/core';
import { Routes, RouterModule } from '@angular/router';
import { HomeComponent } from './content/home/home.component';
import { AlbumsCatalogueComponent } from './pages/catalogue/albums-catalogue/albums-catalogue.component';
import { PerformersCatalogueComponent } from './pages/catalogue/performers-catalogue/performers-catalogue.component';
import { GenresCatalogueComponent } from './pages/catalogue/genres-catalogue/genres-catalogue.component';
import { ShouldLoginComponent } from './should-login/should-login.component';
import { FallbackComponent } from './fallback.component';
import { AlbumDetailsComponent } from './pages/catalogue/details/album-details/album-details.component';
import { PerformersDetailsComponent } from './pages/catalogue/details/performers-details/performers-details.component';
import { StreamsCatalogueComponent } from './pages/catalogue/streams-catalogue/streams-catalogue.component';
import { StreamDetailsComponent } from './pages/catalogue/details/stream-details/stream-details.component';
import { TermsAndConditionsComponent } from './content/terms-and-conditions/terms-and-conditions.component';
import { AuthGuardWithForcedLogin } from './core/utils/auth-guard-with-forced-login.service';
import { RoleGuard } from './core/utils/role-guard.service';
import { AuthGuard } from './core/utils/auth-guard.service';

const routes: Routes = [
  {
    path: 'home',
    component: HomeComponent
  },
  {
    path: 'albums',
    component: AlbumsCatalogueComponent
  },
  {
    path: 'album-details/:id',
    component: AlbumDetailsComponent
  },
  {
    path: 'performers',
    component: PerformersCatalogueComponent
  },
  {
    path: 'performer-details/:id',
    component: PerformersDetailsComponent
  },
  {
    path: 'genres',
    component: GenresCatalogueComponent
  },
  {
    path: 'streams',
    component: StreamsCatalogueComponent,
    canActivate: [AuthGuardWithForcedLogin]
  },
  {
    path: 'stream-details/:id',
    component: StreamDetailsComponent,
    canActivate: [AuthGuardWithForcedLogin]
  },
  {
    path: 'should-login',
    component: ShouldLoginComponent
  },
  {
    path: 'terms-and-conditions',
    component: TermsAndConditionsComponent
  },
  {
    path: '',
    redirectTo: 'home',
    pathMatch: 'full'
  },
  {
    path: 'dashboard',
    loadChildren: () => import('./pages/dashboard/dashboard.module').then(m => m.DashboardModule),
    canActivate: [AuthGuard, RoleGuard],
    data: {
      roles: ['STSAdminRole', 'Editor']
    }
  }
];

@NgModule({
  imports: [RouterModule.forRoot(routes)],
  exports: [RouterModule]
})
export class AppRoutingModule { }
