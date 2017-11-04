import { Component, Input } from "@angular/core";
import { Configuration } from "../../models/configuration";
import { ConfigurationService } from "../../services/configuration.service";

@Component({
    selector: 'task-details',
    templateUrl: './task-details.component.html',
    styleUrls: ['./task-details.component.css']
})
export class TaskDetailsComponent {
    @Input() public task: Task;
    public configuration: Configuration;

    public constructor(configurationService: ConfigurationService) {
        this.configuration = configurationService.getConfiguration();
    }
}