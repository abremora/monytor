import { Component, OnInit, Input, Output } from '@angular/core';
import { Validators, FormBuilder, FormGroup } from '@angular/forms';
import { CollectorConfiguration } from '../../models/collector-configuration.model';
import { Subject } from 'rxjs';
import { CollectorConfigurationFormData } from '../../models/collector-configuration-form-data.model';

@Component({
  selector: 'mt-collector-configuration-form',
  templateUrl: './collector-configuration-form.component.html',
  styles: []
})
export class CollectorConfigurationFormComponent implements OnInit {

  @Input() public data: CollectorConfiguration;

  @Output() public save = new Subject<CollectorConfigurationFormData>();

  public collectorConfigurationForm: FormGroup;

  constructor(private fb: FormBuilder) { }

  ngOnInit() {
    this.initForm();
  }
  initForm() {
    this.collectorConfigurationForm = this.fb.group({
      displayName: [this.data ? this.data.displayName : '', Validators.required],
      schedulerAgent: [this.data ? this.data.schedulerAgentId : '', Validators.required],
    });
  }

  public onSubmit() {
    this.save.next({
      displayName: this.collectorConfigurationForm.get('displayName').value,
      schedulerAgent: this.collectorConfigurationForm.get('schedulerAgent').value
    });
  }
}
