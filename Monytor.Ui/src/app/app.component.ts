import { Component } from '@angular/core';

@Component({
  selector: 'mt-root',
  template: `
  <mt-header></mt-header>
  <mt-sidebar></mt-sidebar>
  <section class='mt-content'>
      <router-outlet></router-outlet>
  </section>
  `
})
export class AppComponent {
  title = 'monytor';
}
