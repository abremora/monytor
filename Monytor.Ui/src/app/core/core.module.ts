import { NgModule } from '@angular/core';
import { CommonModule } from '@angular/common';
import { SidebarComponent } from './sidebar/sidebar.component';
import { NavbarComponent } from './navbar/navbar.component';
import { RouterModule } from '@angular/router';
import { BackButtonComponent } from './back-button/back-button.component';
import { NavbarModule, WavesModule, DropdownModule, ButtonsModule, IconsModule } from 'angular-bootstrap-md';

@NgModule({
    declarations: [
        NavbarComponent,
        SidebarComponent,
        BackButtonComponent
    ],
    imports: [
        CommonModule,
        RouterModule,
        NavbarModule,
        ButtonsModule,
        IconsModule,
        DropdownModule.forRoot(),
        WavesModule.forRoot()

    ],
    exports: [
        NavbarComponent,
        SidebarComponent,
        BackButtonComponent]
})
export class CoreModule { }
