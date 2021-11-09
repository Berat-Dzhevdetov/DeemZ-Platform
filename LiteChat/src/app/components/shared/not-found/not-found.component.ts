import { AfterViewInit, Component, OnInit } from '@angular/core';
import {
  trigger,
  state,
  style,
  animate,
  transition,
} from '@angular/animations';
import { interval } from 'rxjs';

@Component({
  selector: 'app-not-found',
  templateUrl: './not-found.component.html',
  styleUrls: ['./not-found.component.css'],
  animations: [
    trigger('photoState', [
      state('enlarge', style({ transform: 'scale(1.5)' })),
      transition('* => *', animate('500ms ease')),
    ]),
  ],
})
export class NotFoundComponent implements AfterViewInit {
  ngOnInit(): void {}

  ngAfterViewInit() {
    interval(1000)
      .pipe()
      .subscribe(() => {
        this.enlargePic();
      });
    interval(2000)
      .pipe()
      .subscribe(() => {
        this.resetPic();
      });
  }
  position: string = '';

  constructor() {}

  resetPic() {
    this.changePosition('null');
  }

  enlargePic() {
    this.changePosition('enlarge');
  }

  changePosition(newPosition: string) {
    this.position = newPosition;
  }
}
