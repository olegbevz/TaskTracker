import { Pipe, PipeTransform } from '@angular/core';
import { NgbTimeStruct } from "@ng-bootstrap/ng-bootstrap";
import * as moment from "moment";
import { Duration } from "moment";

@Pipe({ name: 'ngbTimeStruct' })
export class NgbTimeStructPipe implements PipeTransform {
    transform(value: string | Duration, args: string[]): NgbTimeStruct|null {
        if (!value) {
            return null;
        }

        let duration: Duration;
        if (moment.isDuration(value)) {
            duration = <Duration>value;
        } else {
            duration = moment.duration(value);
        }
        return <NgbTimeStruct>{
            hour: duration.hours(),
            minute: duration.minutes(),
            second: duration.seconds()
        };
    }

    transformBack(value: NgbTimeStruct, args?: string[]): Duration {
        return moment.duration({
            hours: value.hour,
            minutes: value.minute,
            seconds: value.second
        });
    }
}