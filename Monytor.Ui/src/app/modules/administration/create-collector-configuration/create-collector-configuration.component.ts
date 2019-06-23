import { Component, OnInit } from '@angular/core';
import { CollectorConfigApiService } from '../services/collector-config-api.service';
import { CreateCollectorConfigCommand } from '../models/create-collector-config-command.model';


@Component({
    selector: 'mt-create-collector-configuration',
    templateUrl: './create-collector-configuration.component.html'
})
export class CreateCollectorConfigurationComponent implements OnInit {
    constructor(private apiService: CollectorConfigApiService) { }

    ngOnInit() {

    }

    public onCreate() {
        this.apiService
            .createCollectorConfig(new CreateCollectorConfigCommand('Test', 'default'))
            .toPromise()
            .then(response => console.log(response), error => console.log(error));
    }
}
