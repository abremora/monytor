import { Component, OnInit, Input, Output, EventEmitter, OnDestroy } from '@angular/core';
import { CollectorConfigSearchResult } from 'src/app/modules/administration/models/collector-config-search-result.model';
import { Search } from 'src/app/shared/models/search.model';
import { CollectorConfigApiService } from 'src/app/modules/administration/services/collector-config-api.service';
import { takeUntil, distinct, tap } from 'rxjs/operators';
import { Subject } from 'rxjs';

@Component({
  selector: 'mt-collector-configuration-table',
  templateUrl: './collector-configuration-table.component.html',
  styles: []
})
export class CollectorConfigurationTableComponent implements OnInit, OnDestroy {
  @Input() public collectorConfigurations: Search<CollectorConfigSearchResult>;

  @Output() public collectorDeleted = new EventEmitter<string>();

  private unsubscribe = new Subject();

  constructor(private apiSerivce: CollectorConfigApiService) { }

  ngOnInit() {
  }

  ngOnDestroy() {
    this.unsubscribe.next();
    this.unsubscribe.complete();
  }

  public deleteCollectorConfiguration(id: string) {
    this.apiSerivce.deleteCollectorConfiguration(id)
      .pipe(
        takeUntil(this.unsubscribe),
        distinct(),
        tap(() => {
          // ToDo: May it is better to just mark is as deleted in search result and hide it, so it does not affect totalResults
          this.collectorDeleted.emit(id);
          this.collectorConfigurations.items = this.collectorConfigurations.items.filter(item => item.collectorConfigId !== id);
        })
      )
      .subscribe({
        error: err => console.error(err)
      });
  }

}
