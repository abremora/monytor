import { Component, OnInit, Input } from '@angular/core';
import { CollectorConfigSearchResult } from 'src/app/modules/administration/models/collector-config-search-result.model';
import { Search } from 'src/app/shared/models/search.model';
import { CollectorConfigApiService } from 'src/app/modules/administration/services/collector-config-api.service';
import { take } from 'rxjs/operators';

@Component({
  selector: 'mt-collector-configuration-table',
  templateUrl: './collector-configuration-table.component.html',
  styles: []
})
export class CollectorConfigurationTableComponent implements OnInit {
  @Input() public collectorConfigurations: Search<CollectorConfigSearchResult>;

  constructor(private apiSerivce: CollectorConfigApiService) { }

  ngOnInit() {
  }

  public deleteCollectorConfiguration(id: string) {
    this.apiSerivce.deleteCollectorConfiguration(id).toPromise().then(response => console.log('deleted'), error => console.log(error));
  }

}
