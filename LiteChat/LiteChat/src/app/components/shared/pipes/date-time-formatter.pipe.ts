import { Pipe, PipeTransform } from '@angular/core';
import * as moment from 'moment';

@Pipe({
  name: 'dateTimeFormatter',
})
export class DateTimeFormatterPipe implements PipeTransform {
  transform(dateTime: string): unknown {
    return moment(Date.parse(dateTime)).fromNow();
  }
}
