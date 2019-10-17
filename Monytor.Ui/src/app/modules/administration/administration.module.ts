import { NgModule } from '@angular/core';
import { CommonModule } from '@angular/common';
import { CollectorAdministrationComponent } from './pages/collector-administration/collector-administration.component';
import { AdministrationRoutingModule } from './administration-routing.module';
import { CollectorConfigApiService } from './services/collector-config-api.service';
import { CreateCollectorConfigurationComponent } from './pages/collector-administration/components/create-collector-configuration/create-collector-configuration.component';
import { FormsModule, ReactiveFormsModule } from '@angular/forms';
import { MatFormFieldModule, MatInputModule } from '@angular/material';
import { CollectorConfigurationTableComponent } from './pages/collector-administration/components/collector-configuration-table/collector-configuration-table.component';
import { AddCollectorConfigurationComponent } from './pages/add-collector-configuration/add-collector-configuration.component';
import { EditCollectorConfigurationComponent } from './pages/edit-collector-configuration/edit-collector-configuration.component';

@NgModule({
  declarations: [CollectorAdministrationComponent,
    CreateCollectorConfigurationComponent,
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
  entryComponents: [CollectorAdministrationComponent, CreateCollectorConfigurationComponent]
})
export class AdministrationModule { }
