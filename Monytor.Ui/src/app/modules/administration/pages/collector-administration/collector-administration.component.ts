import { Component, OnInit } from '@angular/core';
import { CollectorConfigApiService } from '../../services/collector-config-api.service';

@Component({
  selector: 'mt-collector-administration',
  templateUrl: './collector-administration.component.html'
})
export class CollectorAdministrationComponent implements OnInit {
  constructor(private apiService: CollectorConfigApiService) {}

  ngOnInit() {
    this.apiService
      .search('*', 1, 10)
      .toPromise()
      .then(response => console.log(response), error => console.log(error));
  }
}
