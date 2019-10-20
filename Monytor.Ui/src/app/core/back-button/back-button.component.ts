import { Component, OnInit } from '@angular/core';
import { Location } from '@angular/common';

@Component({
  selector: 'mt-back-button',
  templateUrl: './back-button.component.html',
  styles: []
})
export class BackButtonComponent implements OnInit {

  constructor(public location: Location) { }

  ngOnInit() {
  }

}
