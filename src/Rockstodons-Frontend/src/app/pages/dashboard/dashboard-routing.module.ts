import { NgModule } from '@angular/core';
import { Routes, RouterModule } from '@angular/router';
import { DashboardComponent } from './dashboard.component';
import { AlbumsManagementComponent } from './albums-management/albums-management.component';
import { GenresManagementComponent } from './genres-management/genres-management.component';
import { AlbumTypesManagementComponent } from './album-types-management/album-types-management.component';
import { PerformersManagementComponent } from './performers-management/performers-management.component';
import { RecycleBinComponent } from './recycle-bin/recycle-bin.component';
import { TracksManagementComponent } from './tracks-management/tracks-management.component';
import { StreamsManagementComponent } from './streams-management/streams-management.component';

const routes: Routes = [
  {
    path: '',
    component: DashboardComponent,
    children: [
      {
        path: 'genres-management',
        component: GenresManagementComponent,
        outlet: 'dashboard'
      },
      {
        path: 'album-types-management',
        component: AlbumTypesManagementComponent,
        outlet: 'dashboard'
      },
      {
        path: 'performers-management',
        component: PerformersManagementComponent,
        outlet: 'dashboard'
      },
      {
        path: 'albums-management',
        component: AlbumsManagementComponent,
        outlet: 'dashboard'
      },
      {
        path: 'tracks-management',
        component: TracksManagementComponent,
        outlet: 'dashboard'
      },
      {
        path: 'streams-management',
        component: StreamsManagementComponent,
        outlet: 'dashboard'
      },
      {
        path: 'recycle-bin',
        component: RecycleBinComponent,
        outlet: 'dashboard'
      }
    ]
  },
];

@NgModule({
  imports: [RouterModule.forChild(routes)],
  exports: [RouterModule]
})
export class DashboardRoutingModule { }
