import { NgModule } from '@angular/core';
import { Routes, RouterModule } from '@angular/router';

const routes: Routes = [

  {
    path: 'collector-administration',
    loadChildren: './collector-administration/collector-administration.module#CollectorAdministrationModule'
  },
  { path: '', redirectTo: 'collector-administration', pathMatch: 'full' }
];

@NgModule({
  imports: [RouterModule.forRoot(routes)],
  exports: [RouterModule]
})
export class AppRoutingModule { }
