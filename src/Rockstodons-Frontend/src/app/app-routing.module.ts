import { NgModule } from '@angular/core';
import { Routes, RouterModule } from '@angular/router';
import { HomeComponent } from './content/home/home.component';
import { AlbumsCatalogueComponent } from './pages/catalogue/albums-catalogue/albums-catalogue.component';
import { PerformersCatalogueComponent } from './pages/catalogue/performers-catalogue/performers-catalogue.component';
import { GenresCatalogueComponent } from './pages/catalogue/genres-catalogue/genres-catalogue.component';
import { ShouldLoginComponent } from './should-login/should-login.component';
import { FallbackComponent } from './fallback.component';
import { AlbumDetailsComponent } from './pages/catalogue/details/album-details/album-details.component';

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
    path: 'genres',
    component: GenresCatalogueComponent
  },
  {
    path: 'should-login',
    component: ShouldLoginComponent
  },
  {
    path: '',
    redirectTo: 'home',
    pathMatch: 'full'
  },
  {
    path: 'dashboard',
    loadChildren: () => import('./pages/dashboard/dashboard.module').then(m => m.DashboardModule)
  }
];

@NgModule({
  imports: [RouterModule.forRoot(routes)],
  exports: [RouterModule]
})
export class AppRoutingModule { }
