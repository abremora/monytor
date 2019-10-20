import { Component, OnInit } from '@angular/core';
import { ActivatedRoute, ParamMap } from '@angular/router';
import { Observable } from 'rxjs';
import { switchMap, tap, map } from 'rxjs/operators';
import { CollectorConfigApiService } from '../../services/collector-config-api.service';
import { CollectorConfiguration } from '../../models/collector-configuration.model';
import { CollectorConfigurationFormData } from '../../models/collector-configuration-form-data.model';
import { EditCollectorConfigCommand } from '../../models/edit-collector-config-command.model';

@Component({
  selector: 'mt-edit-collector-configuration',
  templateUrl: './edit-collector-configuration.component.html',
  styles: []
})
export class EditCollectorConfigurationComponent implements OnInit {
  private collectorConfigId: string;
  public collectorConfiguration$: Observable<CollectorConfiguration>;

  constructor(private route: ActivatedRoute, private apiService: CollectorConfigApiService) { }

  ngOnInit() {
    this.collectorConfiguration$ = this.route.paramMap.pipe(
      map((params: ParamMap) => params.get('id')),
      switchMap(id => {
        this.collectorConfigId = id;
        return this.apiService.getCollectorConfiguration(this.collectorConfigId);
      })
    );
  }

  public onSaveCollectorConfiguration(event: CollectorConfigurationFormData): void {
    this.apiService.editCollectorConfiguration(
      new EditCollectorConfigCommand(this.collectorConfigId, event.displayName, event.schedulerAgent))
      .toPromise()
      .then(respose => console.log('saved'), error => console.log(error));
  }

}
