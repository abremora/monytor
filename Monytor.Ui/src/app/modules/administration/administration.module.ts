import { NgModule } from '@angular/core';
import { CommonModule } from '@angular/common';
import { AdministrationRoutingModule } from './administration-routing.module';
import { CollectorConfigApiService } from './services/collector-config-api.service';
import { FormsModule, ReactiveFormsModule } from '@angular/forms';
// tslint:disable-next-line: max-line-length
import { AddCollectorConfigurationComponent } from './pages/add-collector-configuration/add-collector-configuration.component';
import { EditCollectorConfigurationComponent } from './pages/edit-collector-configuration/edit-collector-configuration.component';
import { CollectorConfigurationTableComponent } from './pages/search-collector-configuration/components/collector-configuration-table/collector-configuration-table.component';
import { SearchCollectorConfigurationComponent } from './pages/search-collector-configuration/search-collector-configuration.component';
import { CoreModule } from 'src/app/core/core.module';
import { CollectorConfigurationFormComponent } from './components/collector-configuration-form/collector-configuration-form.component';
import { MDBBootstrapModule } from 'angular-bootstrap-md';
import { AddCollectorToConfigurationComponent } from './pages/add-collector-to-configuration/add-collector-to-configuration.component';

@NgModule({
  declarations: [
    SearchCollectorConfigurationComponent,
    CollectorConfigurationTableComponent,
    AddCollectorConfigurationComponent,
    EditCollectorConfigurationComponent,
    CollectorConfigurationFormComponent,
    AddCollectorToConfigurationComponent
  ],
  imports: [CommonModule,
    CoreModule,
    AdministrationRoutingModule,
    ReactiveFormsModule,
    MDBBootstrapModule
  ],
  providers: [CollectorConfigApiService],
  entryComponents: [SearchCollectorConfigurationComponent]
})
export class AdministrationModule { }
