import { Component, OnInit } from '@angular/core';
import { NavItem } from './nav-item.model';
import { Router } from '@angular/router';

@Component({
    selector: 'mt-navbar',
    templateUrl: './navbar.component.html',
})
export class NavbarComponent implements OnInit {

    public navItems: NavItem[] = [];
    public activeNavItem: NavItem;

    constructor(private router: Router) {
    }

    public ngOnInit(): void {
        this.navItems.push({ displayText: 'Dashboard', route: '/dashboard' });
        this.navItems.push({ displayText: 'Administration', route: '/administration' });
        // ToDo: Set initial NavItem by current route
        this.activeNavItem = this.navItems[0];
    }

    public navigateTo(navItem: NavItem): void {
        if (navItem === this.activeNavItem) {
            return;
        }
        this.activeNavItem = navItem;
        this.router.navigateByUrl(navItem.route);
    }
}
