import { Pipe, PipeTransform } from '@angular/core';
import * as moment from "moment";
import { Duration } from "moment";

@Pipe({ name: 'durationFormat' })
export class DurationFormatPipe implements PipeTransform {
    transform(value: string | Duration, args: string[]): string | null {
        if (!value) {
            return null;
        }

        let duration: Duration;

        if (moment.isDuration(value)) {
            duration = <Duration>value;
        } else {
            duration = moment.duration(value);
        }

        const hours = duration.hours(),
            minutes = duration.minutes(),
            seconds = duration.seconds();

        return `${hours}h ${minutes}min ${seconds}s`;
    }
}