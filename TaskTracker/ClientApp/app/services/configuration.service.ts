import { Configuration } from "../models/configuration";
import { SystemService } from "./system.service";
import { Inject } from "@angular/core";

export class ConfigurationService {

    private readonly DATETIMEFORMAT_KEY = "TaskTracker_DateTimeFormat";
    private readonly ALTROWSCOLOR_KEY = "TaskTracker_AltRowsColor";

    private readonly defaultConfiguration: Configuration = new Configuration(
        "YYYY-MM-DD HH:mm:ss",
        "#C3C3C3");

    constructor(@Inject(SystemService) private systemService: SystemService) {
    }

    public getConfiguration(): Configuration {
        if (this.systemService.isBrowser()) {
            return new Configuration(
                localStorage.getItem(this.DATETIMEFORMAT_KEY) || this.defaultConfiguration.dateTimeFormat,
                localStorage.getItem(this.ALTROWSCOLOR_KEY) || this.defaultConfiguration.altRowsColor);
        } else {
            return this.defaultConfiguration;
        }
    }

    public saveConfiguration(configuration: Configuration): void {
        if (this.systemService.isBrowser()) {
            localStorage.setItem(this.DATETIMEFORMAT_KEY, configuration.dateTimeFormat);
            localStorage.setItem(this.ALTROWSCOLOR_KEY, configuration.altRowsColor);
        } 
    }
}