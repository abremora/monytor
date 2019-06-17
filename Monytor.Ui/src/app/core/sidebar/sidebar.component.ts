import { Component, OnInit } from '@angular/core';
import { Router } from '@angular/router';
import { NavigationLink } from '../../shared/models/navigation-link.model';

@Component({
  selector: 'mt-sidebar',
  templateUrl: './sidebar.component.html',
})
export class SidebarComponent implements OnInit {
  public navigationLinks: NavigationLink[] = [
    new NavigationLink('/dashboard', 'Dashboard'),
    new NavigationLink('/administration', 'Administration'),
  ];

  public activeNavigationLink: NavigationLink;

  constructor(private router: Router) { }

  ngOnInit() {
    this.activeNavigationLink = this.navigationLinks[0];
  }

  public onLinkClicked(navigationLink: NavigationLink) {
    this.activeNavigationLink = navigationLink;
    this.router.navigateByUrl(navigationLink.linkUrl);
  }

}
