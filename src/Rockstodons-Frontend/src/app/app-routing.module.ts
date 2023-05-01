import { NgModule } from '@angular/core';
import { Routes, RouterModule } from '@angular/router';
import { LoginComponent } from './auth/login/login.component';
import { RegisterComponent } from './auth/register/register.component';
import { HomeComponent } from './content/home/home.component';
import { AuthGuard } from './core/guards/auth.guard';
import { AlbumsCatalogueComponent } from './pages/catalogue/albums-catalogue/albums-catalogue.component';
import { PerformersCatalogueComponent } from './pages/catalogue/performers-catalogue/performers-catalogue.component';
import { GenresCatalogueComponent } from './pages/catalogue/genres-catalogue/genres-catalogue.component';

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
    component: HomeComponent,
    canActivate: [AuthGuard]
  },
  {
    path: 'albums',
    component: AlbumsCatalogueComponent,
    canActivate: [AuthGuard]
  },
  {
    path: 'performers',
    component: PerformersCatalogueComponent,
    canActivate: [AuthGuard]
  },
  {
    path: 'genres',
    component: GenresCatalogueComponent,
    canActivate: [AuthGuard]
  },
  {
    path: '',
    pathMatch: 'full',
    component: HomeComponent,
    canActivate: [AuthGuard]
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
