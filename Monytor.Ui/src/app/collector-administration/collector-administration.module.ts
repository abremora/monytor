import { NgModule } from '@angular/core';
import { CommonModule } from '@angular/common';
import { CollectorAdministrationComponent } from './collector-administration.component';
import { CollectorAdministrationRoutingModule } from './collector-administration-routing.module';

@NgModule({
  declarations: [CollectorAdministrationComponent],
  imports: [
    CommonModule,
    CollectorAdministrationRoutingModule
  ],
  entryComponents: [CollectorAdministrationComponent]
})
export class CollectorAdministrationModule { }
