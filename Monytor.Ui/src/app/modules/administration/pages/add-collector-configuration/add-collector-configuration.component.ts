import { Component, OnInit, OnDestroy } from '@angular/core';
import { FormBuilder, Validators } from '@angular/forms';
import { CollectorConfigApiService } from '../../services/collector-config-api.service';
import { CreateCollectorConfigCommand } from '../../models/create-collector-config-command.model';
import { CollectorConfigurationFormData } from '../../models/collector-configuration-form-data.model';
import { take, tap, takeUntil } from 'rxjs/operators';
import { Router } from '@angular/router';
import { Subject } from 'rxjs';

@Component({
  selector: 'mt-add-collector-configuration',
  templateUrl: './add-collector-configuration.component.html',
  styles: []
})
export class AddCollectorConfigurationComponent implements OnInit, OnDestroy {


  private unsubscribe = new Subject();

  constructor(private apiService: CollectorConfigApiService, private router: Router) { }

  ngOnInit() {
  }

  ngOnDestroy() {
    this.unsubscribe.next();
    this.unsubscribe.complete();
  }

  public onSubmit(event: CollectorConfigurationFormData) {
    this.apiService
      .createCollectorConfig(new CreateCollectorConfigCommand(
        event.displayName
        , event.schedulerAgent))
      .pipe(
        takeUntil(this.unsubscribe),
        take(1),
        tap(id => {
          this.router.navigateByUrl(`/administration/edit/${encodeURIComponent(id)}`, {
            replaceUrl: true
          });
        }),
      ).subscribe({
        error: (err) => console.error(err)
      });
  }

}
