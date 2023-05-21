import { NgModule } from '@angular/core';

import { CommonModule } from '@angular/common';

import { HttpClientModule } from '@angular/common/http';
import { FormsModule, ReactiveFormsModule } from '@angular/forms';
import { NzFormModule } from 'ng-zorro-antd/form';
import { NzInputModule } from 'ng-zorro-antd/input';
import { NzButtonModule } from 'ng-zorro-antd/button';

import { NZ_I18N } from 'ng-zorro-antd/i18n';
import { en_US } from 'ng-zorro-antd/i18n';
import { registerLocaleData } from '@angular/common';
import en from '@angular/common/locales/en';
import { NzLayoutModule } from 'ng-zorro-antd/layout';
import { NzMenuModule } from 'ng-zorro-antd/menu';
import { NzBreadCrumbModule } from 'ng-zorro-antd/breadcrumb';
import { NzTableModule } from 'ng-zorro-antd/table';
import { NzRadioModule } from 'ng-zorro-antd/radio';
import { NzSwitchModule } from 'ng-zorro-antd/switch';
import { NzDividerModule } from 'ng-zorro-antd/divider';
import { NzCheckboxModule } from 'ng-zorro-antd/checkbox';
import { NzPaginationModule } from 'ng-zorro-antd/pagination';
import { NzModalModule } from 'ng-zorro-antd/modal';
import { NzDropDownModule } from 'ng-zorro-antd/dropdown';
import { NzAutocompleteModule } from 'ng-zorro-antd/auto-complete';
import { NzInputNumberModule } from 'ng-zorro-antd/input-number';
import { NzCollapseModule } from 'ng-zorro-antd/collapse';
import { NzListModule } from 'ng-zorro-antd/list';
import { NzSpinModule } from 'ng-zorro-antd/spin';
import { NzSelectModule } from 'ng-zorro-antd/select';
import { NzUploadModule } from 'ng-zorro-antd/upload';
import { NzMessageModule } from 'ng-zorro-antd/message';
import { DashboardComponent } from './dashboard.component';
import { DashboardRoutingModule } from './dashboard-routing.module';
import { IconsProviderModule } from 'src/app/icons-provider.module';
import { AlbumsManagementComponent } from './albums-management/albums-management.component';
import { SharedModule } from 'src/app/common/shared/shared.module';
import { GenresManagementComponent } from './genres-management/genres-management.component';
import { AlbumTypesManagementComponent } from './album-types-management/album-types-management.component';
import { PerformersManagementComponent } from './performers-management/performers-management.component';
import { RecycleBinComponent } from './recycle-bin/recycle-bin.component';
import { AddOrEditAlbumTypeFormComponent } from './forms/add-or-edit-album-type-form/add-or-edit-album-type-form.component';
import { TracksManagementComponent } from './tracks-management/tracks-management.component';
import { StreamsManagementComponent } from './streams-management/streams-management.component';

registerLocaleData(en);

@NgModule({
  declarations: [
    DashboardComponent,
    AlbumsManagementComponent,
    GenresManagementComponent,
    AlbumTypesManagementComponent,
    PerformersManagementComponent,
    RecycleBinComponent,
    AddOrEditAlbumTypeFormComponent,
    TracksManagementComponent,
    StreamsManagementComponent
  ],
  imports: [
    DashboardRoutingModule,
    CommonModule,
    HttpClientModule,
    FormsModule,
    ReactiveFormsModule,
    NzFormModule,
    NzInputModule,
    NzTableModule,
    NzRadioModule,
    NzDividerModule,
    NzButtonModule,
    NzSwitchModule,
    NzCheckboxModule,
    NzPaginationModule,
    NzModalModule,
    NzDropDownModule,
    NzAutocompleteModule,
    NzInputNumberModule,
    NzCollapseModule,
    NzListModule,
    NzSpinModule,
    NzSelectModule,
    NzUploadModule,
    NzMessageModule,
    IconsProviderModule,
    NzLayoutModule,
    NzMenuModule,
    NzBreadCrumbModule,
    SharedModule
  ],
  providers: [
    { provide: NZ_I18N, useValue: en_US }
  ],
  bootstrap: [DashboardComponent]
})
export class DashboardModule { }
