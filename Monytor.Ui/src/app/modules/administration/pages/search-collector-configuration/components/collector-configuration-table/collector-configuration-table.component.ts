import { Component, OnInit, Input } from '@angular/core';
import { CollectorConfigSearchResult } from 'src/app/modules/administration/models/collector-config-search-result.model';
import { Search } from 'src/app/shared/models/search.model';

@Component({
  selector: 'mt-collector-configuration-table',
  templateUrl: './collector-configuration-table.component.html',
  styles: []
})
export class CollectorConfigurationTableComponent implements OnInit {
  @Input() public collectorConfigurations: Search<CollectorConfigSearchResult>;

  constructor() { }

  ngOnInit() {
  }

}
