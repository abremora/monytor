import { NgModule } from '@angular/core';
import { CommonModule } from '@angular/common';
import { CollectorAdministrationComponent } from './pages/collector-administration/collector-administration.component';
import { AdministrationRoutingModule } from './administration-routing.module';
import { CollectorConfigApiService } from './services/collector-config-api.service';
import { CreateCollectorConfigurationComponent } from './create-collector-configuration/create-collector-configuration.component';

@NgModule({
  declarations: [CollectorAdministrationComponent, CreateCollectorConfigurationComponent],
  imports: [CommonModule, AdministrationRoutingModule],
  providers: [CollectorConfigApiService],
  entryComponents: [CollectorAdministrationComponent, CreateCollectorConfigurationComponent]
})
export class AdministrationModule { }
