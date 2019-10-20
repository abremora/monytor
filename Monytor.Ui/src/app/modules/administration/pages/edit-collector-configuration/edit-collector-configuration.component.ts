import { Component, OnInit } from '@angular/core';
import { ActivatedRoute, ParamMap } from '@angular/router';
import { Observable } from 'rxjs';
import { switchMap } from 'rxjs/operators';
import { CollectorConfigApiService } from '../../services/collector-config-api.service';
import { CollectorConfiguration } from '../../models/collector-configuration.model';

@Component({
  selector: 'mt-edit-collector-configuration',
  templateUrl: './edit-collector-configuration.component.html',
  styles: []
})
export class EditCollectorConfigurationComponent implements OnInit {
  public collectorConfiguration$: Observable<CollectorConfiguration>;

  constructor(private route: ActivatedRoute, private apiService: CollectorConfigApiService) { }

  ngOnInit() {
    this.collectorConfiguration$ = this.route.paramMap.pipe(
      switchMap((params: ParamMap) =>
        this.apiService.getCollectorConfiguration(params.get('id')))
    );
  }

}
