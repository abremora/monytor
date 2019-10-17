import { Component, OnInit, OnDestroy } from '@angular/core';
import { CollectorConfigApiService } from '../../services/collector-config-api.service';
import { Subject, Observable } from 'rxjs';
import { tap, distinct, filter, takeUntil, switchMap, map } from 'rxjs/operators';
import { CollectorConfigSearchResult } from '../../models/collector-config-search-result.model';
import { Search } from 'src/app/shared/models/search.model';

@Component({
  selector: 'mt-collector-administration',
  templateUrl: './collector-administration.component.html'
})
export class CollectorAdministrationComponent implements OnInit, OnDestroy {


  private searchTermSubject = new Subject<string>();
  private unsubscribe = new Subject();

  public collectorConfigurations$: Observable<Search<CollectorConfigSearchResult>>;

  constructor(private apiService: CollectorConfigApiService) { }


  ngOnInit() {
    this.collectorConfigurations$ = this.searchTermSubject
      .pipe(
        distinct(),
        filter(x => x.length >= 2 || x.length === 0),
        takeUntil(this.unsubscribe),
        switchMap(term => this.apiService.search(term, 1, 25)),
      );
  }

  ngOnDestroy(): void {
    this.unsubscribe.next();
    this.unsubscribe.complete();
  }

  public onSearchValueChange(searchTerm: string) {
    this.searchTermSubject.next(searchTerm);
  }
}
