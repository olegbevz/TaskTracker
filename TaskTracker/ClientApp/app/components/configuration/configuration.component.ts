import { Component, OnInit } from "@angular/core";
import { ConfigurationService } from "../../services/configuration.service";
import { Configuration } from "../../models/configuration";
import * as moment from "moment";

@Component({
    selector: 'configuration',
    templateUrl: './configuration.component.html',
    styleUrls: ['configuration.component.css']
})
export class ConfigurationComponent implements OnInit {   

    public configuration: Configuration;
    public completed: boolean;

    public moment = moment;

    public constructor(private configurationService: ConfigurationService) {
        this.configuration = this.configurationService.getConfiguration();
    }

    ngOnInit(): void {
        
    }

    onSubmit() {
        this.configurationService.saveConfiguration(this.configuration);
        this.completed = true;
    }
}
