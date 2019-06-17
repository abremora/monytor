import { NgModule } from '@angular/core';
import { CommonModule } from '@angular/common';
import { CollectorAdministrationComponent } from './pages/collector-administration/collector-administration.component';
import { AdministrationRoutingModule } from './administration-routing.module';

@NgModule({
  declarations: [CollectorAdministrationComponent],
  imports: [
    CommonModule,
    AdministrationRoutingModule
  ],
  entryComponents: [CollectorAdministrationComponent]
})
export class AdministrationModule { }
