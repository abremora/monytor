import { NgModule } from '@angular/core';
import { Routes, RouterModule } from '@angular/router';
import { CollectorAdministrationComponent } from './pages/collector-administration/collector-administration.component';

const routes: Routes = [{
    path: '', component: CollectorAdministrationComponent
}];

@NgModule({
    imports: [RouterModule.forChild(routes)],
    exports: [RouterModule]
})
export class AdministrationRoutingModule { }