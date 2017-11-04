import { Pipe, PipeTransform } from '@angular/core';
import * as moment from "moment";
import { Moment } from "moment";

@Pipe({ name: 'momentFormat' })
export class MomentFormatPipe implements PipeTransform {
    transform(value: string | Moment, format: string): string | null {
        if (!value || !format) {
            return null;
        }
        
        let date: Moment;

        if (moment.isMoment(value)) {
            date = <Moment>value;
        } else {
            date = moment(value);
        }

        return date.format(format);
    }
}