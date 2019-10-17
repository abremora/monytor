import { Component, OnInit } from '@angular/core';
import { FormBuilder, Validators } from '@angular/forms';
import { CollectorConfigApiService } from '../../services/collector-config-api.service';
import { CreateCollectorConfigCommand } from '../../models/create-collector-config-command.model';

@Component({
  selector: 'mt-add-collector-configuration',
  templateUrl: './add-collector-configuration.component.html',
  styles: []
})
export class AddCollectorConfigurationComponent implements OnInit {

  public collectorConfigurationForm = this.fb.group({
    displayName: ['', Validators.required],
    schedulerAgent: ['', Validators.required],
  });

  constructor(private fb: FormBuilder, private apiService: CollectorConfigApiService) { }

  ngOnInit() {
  }

  public onSubmit() {
    this.apiService
      .createCollectorConfig(new CreateCollectorConfigCommand(
        this.collectorConfigurationForm.get('displayName').value
        , this.collectorConfigurationForm.get('schedulerAgent').value))
      .toPromise()
      .then(response => console.log(response), error => console.log(error));
  }

}
