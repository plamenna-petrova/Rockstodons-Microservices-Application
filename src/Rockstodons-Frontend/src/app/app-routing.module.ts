import { NgModule } from '@angular/core';
import { Routes, RouterModule } from '@angular/router';
import { LoginComponent } from './auth/login/login.component';
import { RegisterComponent } from './auth/register/register.component';
import { HomeComponent } from './content/home/home.component';
import { AuthGuard } from './core/guards/auth.guard';
import { AlbumsCatalogueComponent } from './pages/catalogue/albums-catalogue/albums-catalogue.component';
import { PerformersCatalogueComponent } from './pages/catalogue/performers-catalogue/performers-catalogue.component';
import { GenresCatalogueComponent } from './pages/catalogue/genres-catalogue/genres-catalogue.component';
import { ShouldLoginComponent } from './should-login/should-login.component';

const routes: Routes = [
  {
    path: 'login',
    component: LoginComponent
  },
  {
    path: 'register',
    component: RegisterComponent
  },
  {
    path: 'home',
    component: HomeComponent
  },
  {
    path: 'albums',
    component: AlbumsCatalogueComponent
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
    pathMatch: 'full',
    component: HomeComponent
  },
  {
    path: 'dashboard',
    loadChildren: () => import('./pages/dashboard/dashboard.module').then(m => m.DashboardModule),
    canActivate: [AuthGuard]
  }
];

@NgModule({
  imports: [RouterModule.forRoot(routes)],
  exports: [RouterModule]
})
export class AppRoutingModule { }
