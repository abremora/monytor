import { HttpClient } from '@angular/common/http';
import { Injectable, InjectionToken, Inject } from '@angular/core';
import { CollectorConfigSearchResult } from '../models/collector-config-search-result.model';
import { Search } from 'src/app/shared/models/search.model';
import { CreateCollectorConfigCommand } from '../models/create-collector-config-command.model';

@Injectable()
export class CollectorConfigApiService {
  constructor(
    @Inject('MonytorWebApiUrl') private apiUrl: string,
    private httpClient: HttpClient
  ) { }

  public search(searchTerms: string, page = 1, pageSize = 10) {
    return this.httpClient.get<Search<CollectorConfigSearchResult>>(
      `${
      this.apiUrl
      }/collectorconfig/search?searchTerms=${searchTerms}&page=${page}&pageSize=${pageSize}`
    );
  }

  public createCollectorConfig(command: CreateCollectorConfigCommand) {
    return this.httpClient.post<string>(`${this.apiUrl}/collectorconfig`, command, {
      responseType: 'text' as 'json'
    });
  }
}
