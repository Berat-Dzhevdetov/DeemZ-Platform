import { Pipe, PipeTransform } from '@angular/core';

@Pipe({
  name: 'shortenText',
})
export class ShortenTextPipe implements PipeTransform {
  transform(text: string, length: number): string {
    return text.length > length ? `${text.substring(0, length)}...` : text;
  }
}