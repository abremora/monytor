import { Component, OnInit, OnDestroy, Input } from '@angular/core';
import { FormBuilder, Validators } from '@angular/forms';
import { CollectorConfigApiService } from '../../services/collector-config-api.service';
import { CreateCollectorConfigCommand } from '../../models/create-collector-config-command.model';
import { CollectorConfigurationFormData } from '../../models/collector-configuration-form-data.model';
import { take, tap, takeUntil } from 'rxjs/operators';
import { Router } from '@angular/router';
import { Subject } from 'rxjs';
import { CollectorConfiguration } from '../../models/collector-configuration.model';

@Component({
  selector: 'mt-add-collector-to-configuration',
  templateUrl: './add-collector-to-configuration.component.html',
  styles: []
})
export class AddCollectorToConfigurationComponent implements OnInit, OnDestroy {

  @Input() public collectorConfiguration: CollectorConfiguration;

  private unsubscribe = new Subject();

  constructor(private apiService: CollectorConfigApiService, private router: Router) { }

  ngOnInit() {
  }

  ngOnDestroy() {
    this.unsubscribe.next();
    this.unsubscribe.complete();
  }

  public onSubmit(event: CollectorConfigurationFormData) {

  }

}
