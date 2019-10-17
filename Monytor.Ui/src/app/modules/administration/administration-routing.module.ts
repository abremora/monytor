import { NgModule } from '@angular/core';
import { Routes, RouterModule } from '@angular/router';
import { AddCollectorConfigurationComponent } from './pages/add-collector-configuration/add-collector-configuration.component';
import { EditCollectorConfigurationComponent } from './pages/edit-collector-configuration/edit-collector-configuration.component';
import { SearchCollectorConfigurationComponent } from './pages/search-collector-configuration/search-collector-configuration.component';

const routes: Routes = [
    {
        path: '',
        children: [
            { path: 'search', component: SearchCollectorConfigurationComponent },
            { path: 'add', component: AddCollectorConfigurationComponent },
            { path: 'edit/:id', component: EditCollectorConfigurationComponent },
            { path: '', redirectTo: 'search', pathMatch: 'full' }
        ]
    }
];

@NgModule({
    imports: [RouterModule.forChild(routes)],
    exports: [RouterModule]
})
export class AdministrationRoutingModule { }
