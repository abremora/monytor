import { Component, OnInit } from '@angular/core';
import { FormBuilder, Validators } from '@angular/forms';
import { CollectorConfigApiService } from '../../services/collector-config-api.service';
import { CreateCollectorConfigCommand } from '../../models/create-collector-config-command.model';
import { CollectorConfigurationFormData } from '../../models/collector-configuration-form-data.model';

@Component({
  selector: 'mt-add-collector-configuration',
  templateUrl: './add-collector-configuration.component.html',
  styles: []
})
export class AddCollectorConfigurationComponent implements OnInit {


  constructor(private apiService: CollectorConfigApiService) { }

  ngOnInit() {
  }

  public onSubmit(event: CollectorConfigurationFormData) {
    this.apiService
      .createCollectorConfig(new CreateCollectorConfigCommand(
        event.displayName
        , event.schedulerAgent))
      .toPromise()
      .then(response => console.log(response), error => console.log(error));
  }

}
