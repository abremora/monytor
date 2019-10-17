import { NgModule } from '@angular/core';
import { CommonModule } from '@angular/common';
import { AdministrationRoutingModule } from './administration-routing.module';
import { CollectorConfigApiService } from './services/collector-config-api.service';
import { FormsModule, ReactiveFormsModule } from '@angular/forms';
import { MatFormFieldModule, MatInputModule } from '@angular/material';
// tslint:disable-next-line: max-line-length
import { AddCollectorConfigurationComponent } from './pages/add-collector-configuration/add-collector-configuration.component';
import { EditCollectorConfigurationComponent } from './pages/edit-collector-configuration/edit-collector-configuration.component';
import { CollectorConfigurationTableComponent } from './pages/search-collector-configuration/components/collector-configuration-table/collector-configuration-table.component';
import { SearchCollectorConfigurationComponent } from './pages/search-collector-configuration/search-collector-configuration.component';

@NgModule({
  declarations: [
    SearchCollectorConfigurationComponent,
    CollectorConfigurationTableComponent,
    AddCollectorConfigurationComponent,
    EditCollectorConfigurationComponent
  ],
  imports: [CommonModule,

    AdministrationRoutingModule,
    ReactiveFormsModule,
    MatFormFieldModule,
    MatInputModule
  ],
  providers: [CollectorConfigApiService],
  entryComponents: [SearchCollectorConfigurationComponent]
})
export class AdministrationModule { }
